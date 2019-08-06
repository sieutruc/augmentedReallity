/*==============================================================================
            Copyright (c) 2010 QUALCOMM Incorporated.
            All Rights Reserved.
            Qualcomm Confidential and Proprietary
            
@file 
    ImageTargets.cpp

@brief
    Sample for ImageTargets

==============================================================================*/


#include "ImageTargets.h"


#ifdef __cplusplus
extern "C"
{
#endif

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_nativeTouchEvent(JNIEnv* env, jobject obj, jint actionType, jint pointerId, jfloat x, jfloat y)
{
    TouchEvent* touchEvent;
    
    // Determine which finger this event represents
    if (pointerId == 0) {
        touchEvent = &touch1;
    } else if (pointerId == 1) {
        touchEvent = &touch2;
    } else {
        return;
    }
    
    if (actionType == ACTION_DOWN) {
        // On touch down, reset the following:
        touchEvent->lastX = x;
        touchEvent->lastY = y;
        touchEvent->startX = x;
        touchEvent->startY = y;
        touchEvent->startTime = getCurrentTimeMS();
        touchEvent->didTap = false;
    } else {
        // Store the last event's position
        touchEvent->lastX = touchEvent->x;
        touchEvent->lastY = touchEvent->y;
    }
    
    // Store the lifetime of the touch, used for tap recognition
    unsigned long time = getCurrentTimeMS();
    touchEvent->dt = time - touchEvent->startTime;
    
    // Store the distance squared from the initial point, for tap recognition
    float dx = touchEvent->lastX - touchEvent->startX;
    float dy = touchEvent->lastY - touchEvent->startY;
    touchEvent->dist2 = dx * dx + dy * dy;
    
    if (actionType == ACTION_UP) {
        // On touch up, this touch is no longer active
        touchEvent->isActive = false;
        
        // Determine if this touch up ends a tap gesture
        // The tap must be quick and localized
        if (touchEvent->dt < MAX_TAP_TIMER && touchEvent->dist2 < MAX_TAP_DISTANCE2) {
            touchEvent->didTap = true;
            touchEvent->tapX = touchEvent->startX;
            touchEvent->tapY = touchEvent->startY;
        }
    } else {
        // On touch down or move, this touch is active
        touchEvent->isActive = true;
    }
    
    // Set the touch information for this event
    touchEvent->actionType = actionType;
    touchEvent->pointerId = pointerId;
    touchEvent->x = x;
    touchEvent->y = y;
}


JNIEXPORT int JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_getOpenGlEsVersionNative(JNIEnv *, jobject)
{
#ifdef USE_OPENGL_ES_1_1        
    return 1;
#else
    return 2;
#endif
}

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_setActivityPortraitMode(JNIEnv *, jobject, jboolean isPortrait)
{
    isActivityInPortraitMode = isPortrait;
}

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_onQCARInitializedNative(JNIEnv *, jobject)
{
    // Comment in to enable tracking of up to 2 targets simultaneously and
    // split the work over multiple frames:
     QCAR::setHint(QCAR::HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS, 2);
     QCAR::setHint(QCAR::HINT_IMAGE_TARGET_MULTI_FRAME_ENABLED, 1);
}

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_GUIManager_nativeHome(JNIEnv*, jobject)
{
	currentPage = 0;
}

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_GUIManager_nativeEnd(JNIEnv*, jobject)
{
	currentPage = numPage - 1;
}

// convert the media's coordinates from marker m1 to marker m2
void convertCoord(Media *media, MarkerInfo *m1, MarkerInfo *m2)
{
	// compute the center of markers
	float x1 = m1->x + m1->width / 2;
	float y1 = -(m1->y + m1->height / 2);
	float x2 = m2->x + m2->width / 2;
	float y2 = -(m2->y + m2->height / 2);
	// The vector that connects the centers of the markers is then (x1 - x2, y1 - y2)
	// And use vector addition we can find the coordinate according to the new marker
	
	media->setXForDrawing(media->getX() + x1 - x2);
	media->setYForDrawing(media->getY() + y1 - y2);
}

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargetsRenderer_renderFrame(JNIEnv *env, jobject)
{
    //LOG("Java_com_qualcomm_QCARSamples_ImageTargets_GLRenderer_renderFrame");
	
    // Clear color and depth buffer 
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

    // Render video background:
    QCAR::State state = QCAR::Renderer::getInstance().begin();
        
#ifdef USE_OPENGL_ES_1_1
    // Set GL11 flags:
    glEnableClientState(GL_VERTEX_ARRAY);
    //glEnableClientState(GL_NORMAL_ARRAY);
    glEnableClientState(GL_TEXTURE_COORD_ARRAY);

    glEnable(GL_TEXTURE_2D);
    glDisable(GL_LIGHTING);
        
#endif

    glEnable(GL_DEPTH_TEST);
    glEnable(GL_CULL_FACE);

    // Did we find any trackables this frame?
	if(state.getNumActiveTrackables() > 0)
	//for(int tIdx = 0; tIdx < state.getNumActiveTrackables(); tIdx++)
    {
        // Get the trackable:
		const QCAR::Trackable* trackable = state.getActiveTrackable(0);
		modelViewMatrix =
            QCAR::Tool::convertPose2GLMatrix(trackable->getPose());        
	
		int id, k;
		for(k = 0; k < numPage; k++)
		{
			id = findMarkerInfo(trackable->getName(), pages[k]);
			if(id != -1)
				break;
		}
		base = pages[k]->getMarker(id);
		currentPage = k;
		
		// Choose the texture based on the target name:
        char buf[50];
        sprintf(buf, "phut %s", trackable->getName());
        LOG(buf);

#ifdef USE_OPENGL_ES_1_1
        // Load projection matrix:
        glMatrixMode(GL_PROJECTION);
        glLoadMatrixf(projectionMatrix.data);

        // Load model view matrix:
        glMatrixMode(GL_MODELVIEW);
        glLoadMatrixf(modelViewMatrix.data);
        glTranslatef(0.f, 0.f, 0.0f);
        glScalef(kObjectScale, kObjectScale, kObjectScale);

        handleTouches(env);
		
		// Draw objects:
		Page *page = pages[currentPage];

		for(int i = page->getSize() - 1; i >= 0; i--)
		{
			Media *media = page->getMedia(i);		
			
			if(strcmp(media->getMarker(), trackable->getName()))
			{
				int mIdx = findMarkerInfo(media->getMarker(), page);
				MarkerInfo *marker = page->getMarker(mIdx);
				convertCoord(media, marker, base);
			}

			media->draw(textures);
		}
		
#else

        QCAR::Matrix44F modelViewProjection;

        SampleUtils::translatePoseMatrix(0.0f, 0.0f, kObjectScale,
                                         &modelViewMatrix.data[0]);
        SampleUtils::scalePoseMatrix(kObjectScale, kObjectScale, kObjectScale,
                                     &modelViewMatrix.data[0]);
        SampleUtils::multiplyMatrix(&projectionMatrix.data[0],
                                    &modelViewMatrix.data[0] ,
                                    &modelViewProjection.data[0]);

        glUseProgram(shaderProgramID);
         
        glVertexAttribPointer(vertexHandle, 3, GL_FLOAT, GL_FALSE, 0,
                              (const GLvoid*) &teapotVertices[0]);
        glVertexAttribPointer(normalHandle, 3, GL_FLOAT, GL_FALSE, 0,
                              (const GLvoid*) &teapotNormals[0]);
        glVertexAttribPointer(textureCoordHandle, 2, GL_FLOAT, GL_FALSE, 0,
                              (const GLvoid*) &teapotTexCoords[0]);
        
        glEnableVertexAttribArray(vertexHandle);
        glEnableVertexAttribArray(normalHandle);
        glEnableVertexAttribArray(textureCoordHandle);
        
        glBindTexture(GL_TEXTURE_2D, thisTexture->mTextureID);
        glUniformMatrix4fv(mvpMatrixHandle, 1, GL_FALSE,
                           (GLfloat*)&modelViewProjection.data[0] );
        glDrawElements(GL_TRIANGLES, NUM_TEAPOT_OBJECT_INDEX, GL_UNSIGNED_SHORT,
                       (const GLvoid*) &teapotIndices[0]);
		
        SampleUtils::checkGlError("ImageTargets renderFrame");
#endif
    
}
    glDisable(GL_DEPTH_TEST);

#ifdef USE_OPENGL_ES_1_1        
    glDisable(GL_TEXTURE_2D);
    glDisableClientState(GL_VERTEX_ARRAY);
    //glDisableClientState(GL_NORMAL_ARRAY);
    glDisableClientState(GL_TEXTURE_COORD_ARRAY);
#else
    glDisableVertexAttribArray(vertexHandle);
    //glDisableVertexAttribArray(normalHandle);
    glDisableVertexAttribArray(textureCoordHandle);
#endif

    QCAR::Renderer::getInstance().end();
}



void
configureVideoBackground()
{
    // Get the default video mode:
    QCAR::CameraDevice& cameraDevice = QCAR::CameraDevice::getInstance();
    QCAR::VideoMode videoMode = cameraDevice.
                                getVideoMode(QCAR::CameraDevice::MODE_DEFAULT);

    // Configure the video background
    QCAR::VideoBackgroundConfig config;
    config.mEnabled = true;
    config.mSynchronous = true;
    config.mPosition.data[0] = 0.0f;
    config.mPosition.data[1] = 0.0f;
    
    if (isActivityInPortraitMode)
    {
        //LOG("configureVideoBackground PORTRAIT");
        config.mSize.data[0] = videoMode.mHeight
                                * (screenHeight / (float)videoMode.mWidth);
        config.mSize.data[1] = screenHeight;
    }
    else
    {
        //LOG("configureVideoBackground LANDSCAPE");
        config.mSize.data[0] = screenWidth;
        config.mSize.data[1] = videoMode.mHeight
                            * (screenWidth / (float)videoMode.mWidth);
    }

    // Set the config:
    QCAR::Renderer::getInstance().setVideoBackgroundConfig(config);
}

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_passNumPages(
                            JNIEnv* env, jobject obj, jint numPages)
{
	numPage = numPages;
	pages = new Page*[numPage];
}

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_passPage(
                            JNIEnv* env, jobject obj, jint i, jobject page)
{
	pages[i] = Page::create(env, page);
}

JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_initApplicationNative(
                            JNIEnv* env, jobject obj, jint width, jint height)
{
    LOG("Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_initApplicationNative");
    
    // Store screen dimensions
    screenWidth = width;
    screenHeight = height;
        
    // Handle to the activity class:
    jclass activityClass = env->GetObjectClass(obj);

    jmethodID getTextureCountMethodID = env->GetMethodID(activityClass,
                                                    "getTextureCount", "()I");
    if (getTextureCountMethodID == 0)
    {
        LOG("Function getTextureCount() not found.");
        return;
    }

    textureCount = (int) env->CallObjectMethod(obj, getTextureCountMethodID);    
    if (!textureCount)
    {
        LOG("getTextureCount() returned zero.");
        return;
    }

    textures = new Texture*[textureCount];

    jmethodID getTextureMethodID = env->GetMethodID(activityClass,
        "getTexture", "(I)Lcom/qualcomm/QCARSamples/ImageTargets/Texture;");

    if (getTextureMethodID == 0)
    {
        LOG("Function getTexture() not found.");
        return;
    }

    // Register the textures
    for (int i = 0; i < textureCount; ++i)
    {

        jobject textureObject = env->CallObjectMethod(obj, getTextureMethodID, i); 
        if (textureObject == NULL)
        {
            LOG("GetTexture() returned zero pointer");
            return;
        }

        textures[i] = Texture::create(env, textureObject);
    }
}


JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_deinitApplicationNative(
                                                        JNIEnv* env, jobject obj)
{
    LOG("Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_deinitApplicationNative");

    // Release texture resources
    if (textures != 0)
    {    
        for (int i = 0; i < textureCount; ++i)
        {
            delete textures[i];
            textures[i] = NULL;
        }
    
        delete[]textures;
        textures = NULL;
        
        textureCount = 0;
    }
}


JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_startCamera(JNIEnv *env,
                                                                         jobject)
{
    LOG("Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_startCamera");

    // Initialize the camera:
    if (!QCAR::CameraDevice::getInstance().init())
        return ;

    // Configure the video background
    configureVideoBackground();

    // Select the default mode:
    if (!QCAR::CameraDevice::getInstance().selectVideoMode(
                                QCAR::CameraDevice::MODE_DEFAULT))
        return ;

    // Start the camera:
    if (!QCAR::CameraDevice::getInstance().start())
        return ;

    // Start the tracker:
    QCAR::Tracker::getInstance().start();
 
    // Cache the projection matrix:
    const QCAR::Tracker& tracker = QCAR::Tracker::getInstance();
    const QCAR::CameraCalibration& cameraCalibration =
                                    tracker.getCameraCalibration();
    projectionMatrix = QCAR::Tool::getProjectionGL(cameraCalibration, 2.0f,
                                            2000.0f);

}


JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_stopCamera(JNIEnv *,
                                                                   jobject)
{
    LOG("Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargets_stopCamera");

    QCAR::Tracker::getInstance().stop();

    QCAR::CameraDevice::getInstance().stop();
    QCAR::CameraDevice::getInstance().deinit();
}


JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargetsRenderer_initRendering(
                                                    JNIEnv* env, jobject obj)
{
    LOG("Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargetsRenderer_initRendering");

    // Define clear color
    glClearColor(0.0f, 0.0f, 0.0f, QCAR::requiresAlpha() ? 0.0f : 1.0f);
    
    // Now generate the OpenGL texture objects and add settings
    for (int i = 0; i < textureCount; ++i)
    {
        glGenTextures(1, &(textures[i]->mTextureID));
        glBindTexture(GL_TEXTURE_2D, textures[i]->mTextureID);
        glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
        glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, textures[i]->mWidth,
                textures[i]->mHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE,
                (GLvoid*)  textures[i]->mData);
    }

	int numVids = 1;
	for(int j = 0; j < numPage; j++)
	{
		Page *page = pages[j];
		int k;
		for(k = 0; k < page->getSize(); k++)
		{
			if(page->getMedia(k)->getType() == 2)
				break;
		}
		
		if(k == page->getSize())
			continue;

		Vid *vid = (Vid*) page->getMedia(k);
		vid->videoTexId = textures[textureCount-1]->mTextureID + numVids;

		int wi = 256;
		int he = 256;
		openfile(vid->getUri(), &(vid->info));
		
		drawframeArg(vid->dataFrame, wi, he, 1, &(vid->info));
		
		glGenTextures(1, &(vid->videoTexId));
		glBindTexture(GL_TEXTURE_2D, vid->videoTexId);
		glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, wi, he, 0, GL_RGBA, GL_UNSIGNED_BYTE, (GLvoid*) vid->dataFrame);
		
		numVids++;
	}

#ifndef USE_OPENGL_ES_1_1
  
    shaderProgramID     = SampleUtils::createProgramFromBuffer(cubeMeshVertexShader,
                                                            cubeFragmentShader);

    vertexHandle        = glGetAttribLocation(shaderProgramID,
                                                "vertexPosition");
    normalHandle        = glGetAttribLocation(shaderProgramID,
                                                "vertexNormal");
    textureCoordHandle  = glGetAttribLocation(shaderProgramID,
                                                "vertexTexCoord");
    mvpMatrixHandle     = glGetUniformLocation(shaderProgramID,
                                                "modelViewProjectionMatrix");

#endif

}


JNIEXPORT void JNICALL
Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargetsRenderer_updateRendering(
                        JNIEnv* env, jobject obj, jint width, jint height)
{
    LOG("Java_com_qualcomm_QCARSamples_ImageTargets_ImageTargetsRenderer_updateRendering");
    
    // Update screen dimensions
    screenWidth = width;
    screenHeight = height;

    // Reconfigure the video background
    configureVideoBackground();
}


#ifdef __cplusplus
}
#endif
