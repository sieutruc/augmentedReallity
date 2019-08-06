#include <jni.h>
#include <android/log.h>
#include <stdio.h>
#include <string.h>
#include <assert.h>
#include "SampleUtils.h"
#include "Texture.h"
#include "main.h"
#include "Md2.h"

#ifdef USE_OPENGL_ES_1_1
#include <GLES/gl.h>
#include <GLES/glext.h>
#else
#include <GLES2/gl2.h>
#include <GLES2/gl2ext.h>
#endif

#ifdef __cplusplus
extern "C"
{
#endif
#include "native.h"

class Media {
protected:
	int type;
	unsigned int texId;
	float x;
	float y;
	float xForDrawing;
	float yForDrawing;
	int width;
	int height;
	char *marker;
	jobject mediaObject;
	jclass mediaObjClass;

public:
	Media()
	{
		marker = new char[50];
	}

	~Media()
	{
		delete marker;
	}

	Media(int type, float x, float y, int width, int height, int texId)
	{
		this->type = type;
		this->x = x;
		this->y = y;
		this->width = width;
		this->height = height;
		this->texId = texId;
	}

	int getType()
	{
		return type;
	}

	int getWidth()
	{
		return width;
	}
	
	int getHeight()
	{
		return height;
	}

	float getX()
	{
		return x;
	}

	void setX(float x)
	{
		this->x = x;
	}

	
	void setXForDrawing(float x)
	{
		this->xForDrawing = x;
	}

	float getY()
	{
		return y;
	}

	void setY(float y)
	{
		this->y = y;
	}

	void setYForDrawing(float y)
	{
		this->yForDrawing = y;
	}

	char* getMarker()
	{
		return marker;
	}

	virtual unsigned int getTexID()
	{
		return texId;
	}

	void setTexID(int id)
	{
		texId = id;
	}

	void drawBasic(int textureID)
	{
		float vertice[12] =
			{xForDrawing, yForDrawing - height, xForDrawing + width, yForDrawing - height, xForDrawing, yForDrawing, xForDrawing, yForDrawing, xForDrawing + width, yForDrawing - height, xForDrawing + width, yForDrawing};
		float tex[12] = {0,0,1,0, 0,1, 0,1, 1,0,1,1};

		glBindTexture(GL_TEXTURE_2D, textureID);
		glTexCoordPointer(2, GL_FLOAT, 0, (const GLvoid*) &tex[0]);
		glVertexPointer(2, GL_FLOAT, 0, (const GLvoid*) &vertice[0]);
		glDrawArrays(GL_TRIANGLES, 0, 6);

		xForDrawing = x;
		yForDrawing = y;
	}
	
	virtual void draw(Texture** textures)
	{
		int textureID = textures[texId]->mTextureID;
		drawBasic(textureID);
	}
	
	virtual void create(JNIEnv* env, jobject mediaObject)
	{
		jclass mediaObjClass = env->GetObjectClass(mediaObject);

		jmethodID getTypeMethodId = env->GetMethodID(mediaObjClass , "getType", "()I");
		type = (int) env->CallObjectMethod(mediaObject, getTypeMethodId);  
		
		jmethodID getTexMethodId = env->GetMethodID(mediaObjClass , "getTexId", "()I");
		texId = (unsigned int) env->CallObjectMethod(mediaObject, getTexMethodId); 
		
		jmethodID getXMethodId = env->GetMethodID(mediaObjClass , "getX", "()F");
		x = env->CallFloatMethod(mediaObject, getXMethodId); 
		xForDrawing = x;

		jmethodID getYMethodId = env->GetMethodID(mediaObjClass , "getY", "()F");
		y = env->CallFloatMethod(mediaObject, getYMethodId); 
		yForDrawing = y;
		
		jmethodID getWidthMethodId = env->GetMethodID(mediaObjClass , "getWidth", "()I");
		width = (int) env->CallObjectMethod(mediaObject, getWidthMethodId); 

		jmethodID getHeightMethodId = env->GetMethodID(mediaObjClass , "getHeight", "()I");
		height = (int) env->CallObjectMethod(mediaObject, getHeightMethodId);  

		jmethodID getMarkerMethodId = env->GetMethodID(mediaObjClass , "getMarker", "()Ljava/lang/String;");
		jstring tmp = (jstring) env->CallObjectMethod(mediaObject, getMarkerMethodId); 	
		const char *str = env->GetStringUTFChars(tmp, 0);
		strcpy(marker, str);

		this->mediaObject = env->NewGlobalRef(mediaObject);
		
	}
	
	virtual Media *clone()
	{
		return new Media();
	}

	virtual void handleTouch(JNIEnv *env)
	{
		LOG("touched image");
	}
};

class ImgSeq : public Media {
	int numImgs;
	int curImgId;

public:
	ImgSeq()
	{
		curImgId = 0;
	}
	
	ImgSeq(int type, float x, float y, int width, int height, int texId, int numImgs)
		: Media(type, x, y, width, height, texId)
	{
		this->numImgs = numImgs;
	}
	
	int getCurImgId()
	{
		return curImgId;
	}

	void incCurImgId()
	{
		curImgId = (curImgId + 1) % numImgs;
	}
	
	virtual void create(JNIEnv* env, jobject mediaObject)
	{
		Media::create(env, mediaObject);
		
		jclass mediaObjClass = env->GetObjectClass(mediaObject);

		jmethodID getNumUriMethodId = env->GetMethodID(mediaObjClass , "getNumUri", "()I");
		numImgs = (int) env->CallObjectMethod(mediaObject, getNumUriMethodId); 
	}

	virtual void draw(Texture** textures)
	{
		int textureID = textures[texId + curImgId]->mTextureID;
		drawBasic(textureID);
	}

	virtual Media *clone()
	{
		return new ImgSeq();
	}

	virtual void handleTouch(JNIEnv *env)
	{
		LOG("touched imgseq");
		incCurImgId();
	}
};

class Audio : public Media {
public: 
	bool isPlaying;

public:
	Audio()
	{
		isPlaying = false;
	}

	virtual void draw(Texture** textures)
	{
		int textureID = textures[0]->mTextureID;
		drawBasic(textureID);
	}

	virtual Media *clone()
	{
		return new Audio();
	}

	void play(JNIEnv *env)
	{
		jclass mediaObjClass = env->GetObjectClass(mediaObject);
				
		jmethodID startMethodId = env->GetMethodID(mediaObjClass , "start", "()V");
		env->CallVoidMethod(mediaObject, startMethodId); 
	}

	void pause(JNIEnv *env)
	{
		jclass mediaObjClass = env->GetObjectClass(mediaObject);
		
		jmethodID pauseMethodId = env->GetMethodID(mediaObjClass , "pause", "()V");
		env->CallVoidMethod(mediaObject, pauseMethodId);  
	}

	virtual void handleTouch(JNIEnv *env)
	{
		LOG("touched audio");
		jclass mediaObjClass = env->GetObjectClass(mediaObject);
		
		isPlaying = !isPlaying;
	
		if(isPlaying)
		{			
			play(env);
		}
		else
		{
			pause(env);
		}
	}
};

class Vid : public Media {
public:
	uint8_t *dataFrame;
	char *uri;
	unsigned int videoTexId;
	bool isPlaying;
	AVInfo info;

public:
	Vid()
	{
		dataFrame = new uint8_t[256 * 256 * 4];
		uri = new char[256];
		isPlaying = true;
	}

	~Vid()
	{
		delete dataFrame;
		delete uri;
		deinitialize(&info);
	}
	
	virtual unsigned int getTexID()
	{
		return videoTexId;
	}

	char* getUri()
	{
		return uri;
	}

	virtual void create(JNIEnv* env, jobject mediaObject)
	{
		Media::create(env, mediaObject);
		
		jclass mediaObjClass = env->GetObjectClass(mediaObject);

		jmethodID getUriMethodId = env->GetMethodID(mediaObjClass , "getUri", "(I)Ljava/lang/String;");
		jstring tmp = (jstring) env->CallObjectMethod(mediaObject, getUriMethodId, 0); 
		
		const char *str = env->GetStringUTFChars(tmp, 0);
		strcpy(uri, str);
	}
	
	void draw(Texture** textures)
	{
		int wi = 256, he = 256;
		int end = drawframeArg(dataFrame, wi, he, 1, &info);
		if(end)
		{
			deinitialize(&info);
			openfile(uri, &info);	
			isPlaying = false;
		}
		else
		{
			float vertice[12] =
				{xForDrawing, yForDrawing - height, xForDrawing + width, yForDrawing - height, xForDrawing, yForDrawing, xForDrawing, yForDrawing, xForDrawing + width, yForDrawing - height, xForDrawing + width, yForDrawing};
			float tex[12] = {0,0,1,0, 0,1, 0,1, 1,0,1,1};
					
			glBindTexture(GL_TEXTURE_2D, videoTexId);
			glTexCoordPointer(2, GL_FLOAT, 0, (const GLvoid*) &tex[0]);
			glVertexPointer(2, GL_FLOAT, 0, (const GLvoid*) &vertice[0]);
			glDrawArrays(GL_TRIANGLES, 0, 6);
			
			if(isPlaying)
			{
				//glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, wi, he, 0, GL_RGBA, GL_UNSIGNED_BYTE, (GLvoid*) dataFrame);
				glTexSubImage2D (GL_TEXTURE_2D, 0, 0, 0, wi, he, GL_RGBA,
								GL_UNSIGNED_BYTE, (GLvoid*)dataFrame);								
			}
			xForDrawing = x;
			yForDrawing = y;
		}	
	}

	virtual Media *clone()
	{
		return new Vid();
	}

	virtual void handleTouch(JNIEnv *env)
	{
		LOG("touched vid");
		isPlaying = !isPlaying; 
	}
};

class MD2Model : public Media {
public:
	bool isAnimating;
	CLoadMD2 g_LoadMd2;									
	t3DModel g_3DModel;	
	char *md2Filename;

public:
	MD2Model()
	{
		isAnimating = false;
		md2Filename = new char[256];
	}

	~MD2Model()
	{
		delete md2Filename;
	}

	virtual void create(JNIEnv* env, jobject mediaObject)
	{
		Media::create(env, mediaObject);
		
		jclass mediaObjClass = env->GetObjectClass(mediaObject);

		jmethodID getUriMethodId = env->GetMethodID(mediaObjClass , "getUri", "(I)Ljava/lang/String;");
		jstring tmp = (jstring) env->CallObjectMethod(mediaObject, getUriMethodId, 0); 
		
		const char *str = env->GetStringUTFChars(tmp, 0);
		strcpy(md2Filename, str);

		g_LoadMd2.ImportMD2(&g_3DModel, md2Filename, "fsfs");
		setAnimation(&g_3DModel, "run");
	}

	virtual Media *clone()
	{
		return new MD2Model();
	}

	virtual void draw(Texture** textures)
	{
		if(!isAnimating)
		{
			int textureID = textures[1]->mTextureID;
			drawBasic(textureID);
		}
		else
		{
			int textureID = textures[texId]->mTextureID;
			glBindTexture(GL_TEXTURE_2D, textureID);
			
			glTranslatef(0.f, 0.f, 30.0f);
			glRotatef(70, 1, 0, 0);
			glRotatef(20, 0, 0, 1);
			glScalef(4, 4, 4);
			
			AnimateMD2Model(&g_3DModel);
			
			glScalef(1/4, 1/4, 1/4);
			glRotatef(-70, 1, 0, 0);
			glRotatef(-20, 0, 0, 1);
			glTranslatef(0.f, 0.f, -30.0f);			
		}
	}

	virtual void handleTouch(JNIEnv *env)
	{
		LOG("touched MD2");
		isAnimating = !isAnimating;
	}

	unsigned GetTickCount()
	{
        struct timeval tv;
        if(gettimeofday(&tv, NULL) != 0)
                return 0;		
        return (tv.tv_sec * 1000) + (tv.tv_usec / 1000);
	}

	float ReturnCurrentTime(t3DModel *pModel, int nextFrame)
	{
		static float elapsedTime   = 0.0f;
		static float lastTime	  = 0.0f;

		float time = (float)GetTickCount();
		elapsedTime = time - lastTime;
		float t = elapsedTime / (1000.0f / kAnimationSpeed);
		
		// If our elapsed time goes over a 5th of a second, we start over and go to the next key frame
		if (elapsedTime >= (1000.0f / kAnimationSpeed) )
		{
			pModel->currentFrame = nextFrame;
			lastTime = time;
		}

		return t;
	}

	void setAnimation(t3DModel *pModel, char *name)
	{
		int numAni = pModel->numOfAnimations;
		for(int i = 0; i < numAni; i++)
			if(strcmp(pModel->pAnimations[i].strName, name) == 0)
			{
				pModel->currentAnim = i;
				pModel->currentFrame = pModel->pAnimations[i].startFrame;
				break;
			}
	}


	void AnimateMD2Model(t3DModel *pModel)
	{
		if(pModel->pObject.size() <= 0) 
		{
			return;
		}

		tAnimationInfo *pAnim = &(pModel->pAnimations[pModel->currentAnim]);
		
		int nextFrame = (pModel->currentFrame + 1) % pAnim->endFrame;

		if(nextFrame == 0) 
			nextFrame =  pAnim->startFrame;
		
		t3DObject *pFrame =	&pModel->pObject[pModel->currentFrame];
		
		t3DObject *pNextFrame = &pModel->pObject[nextFrame];

		t3DObject *pFirstFrame = &pModel->pObject[0];
		
		float t = ReturnCurrentTime(pModel, nextFrame);
		
		float texcoords[pFirstFrame->numOfFaces*6];
		float vertices[pFrame->numOfFaces*9];
		
		int idxTex = 0, idxVert = 0;

		// Go through all of the faces (polygons) of the current frame and draw them
		for(int j = 0; j < pFrame->numOfFaces; j++)
		{
			// Go through each corner of the triangle and draw it.
			for(int whichVertex = 0; whichVertex < 3; whichVertex++)
			{
				int vertIndex = pFirstFrame->pFaces[j].vertIndex[whichVertex];	
				int texIndex  = pFirstFrame->pFaces[j].coordIndex[whichVertex];	
					
				if(pFirstFrame->pTexVerts) 
				{
					texcoords[idxTex] = pFirstFrame->pTexVerts[ texIndex ].x;
					texcoords[idxTex+1] = pFirstFrame->pTexVerts[ texIndex ].y;
					idxTex += 2;
				}
					
				// Store the current and next frame's vertex
				CVector3 vPoint1 = pFrame->pVerts[ vertIndex ];
				CVector3 vPoint2 = pNextFrame->pVerts[ vertIndex ];
					
				vertices[idxVert] = vPoint1.x + t * (vPoint2.x - vPoint1.x);
				vertices[idxVert+1] = vPoint1.y + t * (vPoint2.y - vPoint1.y);
				vertices[idxVert+2] = vPoint1.z + t * (vPoint2.z - vPoint1.z);
				idxVert += 3;
			}
		}
			
		glVertexPointer(3, GL_FLOAT, 0, vertices);
		glTexCoordPointer(2, GL_FLOAT, 0, texcoords);
		glDrawArrays(GL_TRIANGLES, 0, pFrame->numOfFaces*3);				
	}
};

class MarkerInfo {
public:
	float width;
	float height;
	float x;
	float y;
	char *name;
	int page;
	
	MarkerInfo()
	{
		name = new char[50];
	}

	~MarkerInfo()
	{
		delete name;
	}
	
	void create(JNIEnv* env, jobject markerObject)
	{
		jclass markerClass = env->GetObjectClass(markerObject);
		
		jmethodID getXMethodId = env->GetMethodID(markerClass , "getX", "()F");
		x = env->CallFloatMethod(markerObject, getXMethodId); 

		jmethodID getYMethodId = env->GetMethodID(markerClass , "getY", "()F");
		y = env->CallFloatMethod(markerObject, getYMethodId); 
		
		jmethodID getWidthMethodId = env->GetMethodID(markerClass , "getWidth", "()F");
		width = env->CallFloatMethod(markerObject, getWidthMethodId); 

		jmethodID getHeightMethodId = env->GetMethodID(markerClass , "getHeight", "()F");
		height = env->CallFloatMethod(markerObject, getHeightMethodId);  

		jmethodID getPageMethodId = env->GetMethodID(markerClass , "getPage", "()I");
		page = (int) env->CallObjectMethod(markerObject, getPageMethodId);  

		jmethodID getNameMethodId = env->GetMethodID(markerClass , "getName", "()Ljava/lang/String;");
		jstring tmp = (jstring) env->CallObjectMethod(markerObject, getNameMethodId); 	
		const char *str = env->GetStringUTFChars(tmp, 0);
		strcpy(name, str);
	}
};

class Page {
	Media **objs;
	MarkerInfo **markers;
	int size;
	int numMarkers;

public:
	Page()
	{
	}

	~Page()
	{
		for(int i = 0; i < size; i++)
			delete objs[i];
		delete objs;

		for(int k = 0; k < numMarkers; k++)
			delete markers[k];
		delete markers;
	}

	Page(int size)
	{
		setSize(size);
	}

	int getSize()
	{
		return size;
	}
	void setSize(int size)
	{
		this->size = size;
		objs = new Media*[size];
	}

	Media* getMedia(int i)
	{
		return objs[i];
	}
	void setMedia(int i, Media *media)
	{
		objs[i] = media;
	}

	int getNumMarkers()
	{
		return numMarkers;
	}

	MarkerInfo* getMarker(int i)
	{
		return markers[i];
	}

	static Page* create(JNIEnv* env, jobject pageObject)
	{
		Page *page = new Page();

		jclass pageClass = env->GetObjectClass(pageObject);

		jmethodID numObjectsMethodId = env->GetMethodID(pageClass , "numObjects", "()I");
		int numObjects = (int) env->CallObjectMethod(pageObject, numObjectsMethodId);  
		page->setSize(numObjects);

		jmethodID numMarkersMethodId = env->GetMethodID(pageClass , "numMarkers", "()I");
		page->numMarkers = (int) env->CallObjectMethod(pageObject, numMarkersMethodId);  
		page->markers = new MarkerInfo*[page->numMarkers];

		for(int i = 0; i < numObjects; i++)
		{
			jmethodID getMethodId = env->GetMethodID(pageClass , "get", "(I)Lcom/qualcomm/QCARSamples/ImageTargets/MediaObject;");
			jobject mediaObject = env->CallObjectMethod(pageObject, getMethodId, i);
			
			jclass mediaClass = env->GetObjectClass(mediaObject);
			jmethodID getMediaTypeMethodId = env->GetMethodID(mediaClass , "getType", "()I");
			int mediaType = (int) env->CallObjectMethod(mediaObject, getMediaTypeMethodId); 

			Media *types[5] = {new Media(), new ImgSeq(), new Vid(), new Audio(), new MD2Model()};
			Media *tmp = types[mediaType];
			page->objs[i] = tmp->clone();
			page->objs[i]->create(env, mediaObject);
		}

		for(int k = 0; k < page->numMarkers; k++)
		{
			jmethodID getMethodId = env->GetMethodID(pageClass , "getMarker", "(I)Lcom/qualcomm/QCARSamples/ImageTargets/Marker;");
			jobject markerObject = env->CallObjectMethod(pageObject, getMethodId, k);
		
			page->markers[k] = new MarkerInfo();
			page->markers[k]->create(env, markerObject);
		}

		return page;
	}
};

#ifdef __cplusplus
}
#endif