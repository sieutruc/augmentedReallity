#include <jni.h>
#include <string.h>
#include <stdio.h>
#include <android/log.h>
#include <android/bitmap.h>

#include <libavcodec/avcodec.h>
#include <libavformat/avformat.h>
#include <libswscale/swscale.h>


struct AVInfo
{
	AVFormatContext *pFormatCtx;
	AVCodecContext *pCodecCtx;
	AVFrame *pFrame;
	AVFrame *pFrameRGB;
	int videoStream;
	uint8_t *buffer;
};

void openfile(char *filename, struct AVInfo *info);
int drawframeArg(uint8_t* data,int w, int h,int numFrame, struct AVInfo *info);
int eof(struct AVInfo *info);
void deinitialize(struct AVInfo *info);
