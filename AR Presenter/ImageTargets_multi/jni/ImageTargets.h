#include <jni.h>
#include <android/log.h>
#include <stdio.h>
#include <string.h>
#include <assert.h>
#include <sys/time.h>
#include <math.h>

#ifdef USE_OPENGL_ES_1_1
#include <GLES/gl.h>
#include <GLES/glext.h>
#else
#include <GLES2/gl2.h>
#include <GLES2/gl2ext.h>
#endif

#include <QCAR/QCAR.h>
#include <QCAR/CameraDevice.h>
#include <QCAR/Renderer.h>
#include <QCAR/VideoBackgroundConfig.h>
#include <QCAR/Trackable.h>
#include <QCAR/Tool.h>
#include <QCAR/Tracker.h>
#include <QCAR/CameraCalibration.h>
#include <QCAR/VirtualButton.h>
#include <QCAR/Rectangle.h>
#include <QCAR/ImageTarget.h>

#include "WebPage.h"
#include "SampleUtils.h"
#include "SampleMath.h"
#include "Texture.h"
#include "CubeShaders.h"

#define MAX_TAP_TIMER 200
#define MAX_TAP_DISTANCE2 400

enum ActionType {
    ACTION_DOWN,
    ACTION_MOVE,
    ACTION_UP,
    ACTION_CANCEL
};

struct TouchEvent {
    bool isActive;
    int actionType;
    int pointerId;
    float x;
    float y;
    float lastX;
    float lastY;
    float startX;
    float startY;
    float tapX;
    float tapY;
    unsigned long startTime;
    unsigned long dt;
    float dist2;
    bool didTap;
};


// Textures:
int textureCount                = 0;
Texture** textures              = 0;

// OpenGL ES 2.0 specific:
#ifdef USE_OPENGL_ES_2_0
unsigned int shaderProgramID    = 0;
GLint vertexHandle              = 0;
GLint normalHandle              = 0;
GLint textureCoordHandle        = 0;
GLint mvpMatrixHandle           = 0;
#endif

// Screen dimensions:
unsigned int screenWidth        = 0;
unsigned int screenHeight       = 0;

// Indicates whether screen is in portrait (true) or landscape (false) mode
bool isActivityInPortraitMode   = false;

// The projection matrix used for rendering virtual objects:
QCAR::Matrix44F projectionMatrix;
QCAR::Matrix44F inverseProjMatrix;
QCAR::Matrix44F modelViewMatrix;

static const float kObjectScale	= 1.f;
Page **pages;
int numPage;
int currentPage = 0;
TouchEvent touch1, touch2;
unsigned long lastTapTime = 0;
MarkerInfo *base;


int findMarkerInfo(const char *name, Page *page)
{
	int numMarkers = page->getNumMarkers();
	for(int i = 0; i < numMarkers; i++)
		if(!strcmp(page->getMarker(i)->name, name))
			return i;
	return -1;
}

unsigned long getCurrentTimeMS() {
    struct timeval tv;
    gettimeofday(&tv, NULL);
    unsigned long s = tv.tv_sec * 1000;
    unsigned long us = tv.tv_usec / 1000;
    return s + us;
}

bool
linePlaneIntersection(QCAR::Vec3F lineStart, QCAR::Vec3F lineEnd,
                      QCAR::Vec3F pointOnPlane, QCAR::Vec3F planeNormal,
                      QCAR::Vec3F &intersection)
{
    QCAR::Vec3F lineDir = SampleMath::Vec3FSub(lineEnd, lineStart);
    lineDir = SampleMath::Vec3FNormalize(lineDir);
    
    QCAR::Vec3F planeDir = SampleMath::Vec3FSub(pointOnPlane, lineStart);
    
    float n = SampleMath::Vec3FDot(planeNormal, planeDir);
    float d = SampleMath::Vec3FDot(planeNormal, lineDir);
    
    if (fabs(d) < 0.00001) {
        // Line is parallel to plane
        return false;
    }
    
    float dist = n / d;
    
    QCAR::Vec3F offset = SampleMath::Vec3FScale(lineDir, dist);
    intersection = SampleMath::Vec3FAdd(lineStart, offset);
}

void
projectScreenPointToPlane(QCAR::Vec2F point, QCAR::Vec3F planeCenter, QCAR::Vec3F planeNormal,
                          QCAR::Vec3F &intersection, QCAR::Vec3F &lineStart, QCAR::Vec3F &lineEnd)
{
    // Window Coordinates to Normalized Device Coordinates
    QCAR::VideoBackgroundConfig config = QCAR::Renderer::getInstance().getVideoBackgroundConfig();
    
    float halfScreenWidth = screenWidth / 2.0f;
    float halfScreenHeight = screenHeight / 2.0f;
    
    float halfViewportWidth = config.mSize.data[0] / 2.0f;
    float halfViewportHeight = config.mSize.data[1] / 2.0f;
    
    float x = (point.data[0] - halfScreenWidth) / halfViewportWidth;
    float y = (point.data[1] - halfScreenHeight) / halfViewportHeight * -1;
    
    QCAR::Vec4F ndcNear(x, y, -1, 1);
    QCAR::Vec4F ndcFar(x, y, 1, 1);
    
    inverseProjMatrix = SampleMath::Matrix44FInverse(projectionMatrix);
	
	// Normalized Device Coordinates to Eye Coordinates
    QCAR::Vec4F pointOnNearPlane = SampleMath::Vec4FTransform(ndcNear, inverseProjMatrix);
    QCAR::Vec4F pointOnFarPlane = SampleMath::Vec4FTransform(ndcFar, inverseProjMatrix);
    pointOnNearPlane = SampleMath::Vec3FDiv(pointOnNearPlane, pointOnNearPlane.data[3]);
    pointOnFarPlane = SampleMath::Vec3FDiv(pointOnFarPlane, pointOnFarPlane.data[3]);
    
    // Eye Coordinates to Object Coordinates
    QCAR::Matrix44F inverseModelViewMatrix = SampleMath::Matrix44FInverse(modelViewMatrix);
    
    QCAR::Vec4F nearWorld = SampleMath::Vec4FTransform(pointOnNearPlane, inverseModelViewMatrix);
    QCAR::Vec4F farWorld = SampleMath::Vec4FTransform(pointOnFarPlane, inverseModelViewMatrix);
    
    lineStart = QCAR::Vec3F(nearWorld.data[0], nearWorld.data[1], nearWorld.data[2]);
    lineEnd = QCAR::Vec3F(farWorld.data[0], farWorld.data[1], farWorld.data[2]);
    linePlaneIntersection(lineStart, lineEnd, planeCenter, planeNormal, intersection);
}

void convertCoord2(float &x, float &y, MarkerInfo *m1, MarkerInfo *m2)
{
	float x1 = m1->x + m1->width / 2;
	float y1 = -(m1->y + m1->height / 2);
	float x2 = m2->x + m2->width / 2;
	float y2 = -(m2->y + m2->height / 2);
	
	x += x1 - x2;
	y += y1 - y2;
}

bool checkPointInRec(float pointX, float pointY, float x, float y, int width, int height)
{
	bool b;
	b = (pointX > x) && (pointX < x + width)
	 && (pointY > y - height) && (pointY < y);
	return b;
}

void handleTouches(JNIEnv *env)
{
	// If there is a new tap that we haven't handled yet:
    if (touch1.didTap && touch1.startTime > lastTapTime) {
        
        // Find the start and end points in world space for the tap
        // These will lie on the near and far plane and can be used for picking
        QCAR::Vec3F intersection, lineStart, lineEnd;
        projectScreenPointToPlane(QCAR::Vec2F(touch1.tapX, touch1.tapY), QCAR::Vec3F(0, 0, 0), QCAR::Vec3F(0, 0, 1), intersection, lineStart, lineEnd);
        
		char buf[50];
		sprintf(buf, "test touch %f %f", intersection.data[0], intersection.data[1]);
		LOG(buf);

		Page *page = pages[currentPage];
		for(int i = 0; i <= page->getSize() - 1; i++)
		{
			Media *media = page->getMedia(i);
			
			float x = media->getX();
			float y = media->getY();
			int w = media->getWidth();
			int h = media->getHeight();
			int id = findMarkerInfo(media->getMarker(), page);
			MarkerInfo *curMarker = page->getMarker(id);
			convertCoord2(x, y, curMarker, base);

			bool b = checkPointInRec(intersection.data[0], intersection.data[1], x, y, w, h);
			if(b)
				media->handleTouch(env);
		}
               
        // Store the timestamp for this tap so we know we've handled it
        lastTapTime = touch1.startTime;
        
    } 
}

