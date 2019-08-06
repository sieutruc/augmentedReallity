

#include "native.h"


#define  LOG_TAG    "FFMPEGSample"
#define  LOGI(...)  __android_log_print(ANDROID_LOG_INFO,LOG_TAG,__VA_ARGS__)
#define  LOGE(...)  __android_log_print(ANDROID_LOG_ERROR,LOG_TAG,__VA_ARGS__)
/*
 * Write a frame worth of video (in pFrame) into the Android bitmap
 * described by info using the raw pixel buffer.  It's a very inefficient
 * draw routine, but it's easy to read. Relies on the format of the
 * bitmap being 8bits per color component plus an 8bit alpha channel.
 */



/*static void fill_bitmap(AndroidBitmapInfo*  info, void *pixels, AVFrame *pFrame)
{
    uint8_t *frameLine;

    int  yy;
    for (yy = 0; yy < info->height; yy++) {
        uint8_t*  line = (uint8_t*)pixels;
        frameLine = (uint8_t *)pFrame->data[0] + (yy * pFrame->linesize[0]);

        int xx;
        for (xx = 0; xx < info->width; xx++) {
            int out_offset = xx * 4;
            int in_offset = xx * 3;

            line[out_offset] = frameLine[in_offset];
            line[out_offset+1] = frameLine[in_offset+1];
            line[out_offset+2] = frameLine[in_offset+2];
            line[out_offset+3] = 0;
        }
        pixels = (char*)pixels + 1280;
    }
}*/

void openfile(char *filename, struct AVInfo *info)
{
	int ret;
	    int err;
	    int i;
	    AVCodec *pCodec;
	    uint8_t *buffer;
	    int numBytes;

	    av_register_all();
	    err = av_open_input_file(&(info->pFormatCtx), filename, NULL, 0, NULL);
	    if(err!=0) {
	        LOGE("Couldn't open file");
	        return;
	    }
	    LOGE("Opened file");

	    if(av_find_stream_info(info->pFormatCtx)<0) {
	        LOGE("Unable to get stream info");
	        return;
	    }

	    info->videoStream = -1;
	    for (i=0; i<info->pFormatCtx->nb_streams; i++) {
	        if(info->pFormatCtx->streams[i]->codec->codec_type==CODEC_TYPE_VIDEO) {
	            info->videoStream = i;
	            break;
	        }
	    }
	    if(info->videoStream==-1) {
	        LOGE("Unable to find video stream");
	        return;
	    }
	    info->pCodecCtx = info->pFormatCtx->streams[info->videoStream]->codec;

	    pCodec=avcodec_find_decoder(info->pCodecCtx->codec_id);
	    if(pCodec==NULL) {
	        LOGE("Unsupported codec");
	        return;
	    }

	    if(avcodec_open(info->pCodecCtx, pCodec)<0) {
	        LOGE("Unable to open codec");
	        return;
	    }

	    info->pFrame=avcodec_alloc_frame();
	    info->pFrameRGB=avcodec_alloc_frame();
	    LOGE("Video size is [%d x %d]", info->pCodecCtx->width, info->pCodecCtx->height);

	    numBytes=avpicture_get_size(PIX_FMT_RGBA, info->pCodecCtx->width, info->pCodecCtx->height);
	    info->buffer=(uint8_t *)av_malloc(numBytes*sizeof(uint8_t));

	    avpicture_fill((AVPicture *)info->pFrameRGB, info->buffer, PIX_FMT_RGBA,
	                            info->pCodecCtx->width, info->pCodecCtx->height);
}

int eof(struct AVInfo *info)
{
	AVPacket packet;
	if (av_read_frame(info->pFormatCtx, &packet)< 0)
		return 1;
	return 0;
}

int drawframeArg(uint8_t* data,int w, int h,int numFrame, struct AVInfo *info)
{
		    int i;
		    int frameFinished = 0;
		    AVPacket packet;
		    static struct SwsContext *img_convert_ctx;

		    i = 0;
		    while((i<numFrame)) 
			if(av_read_frame(info->pFormatCtx, &packet)>=0)
			{
		  		if(packet.stream_index==info->videoStream) {
		            avcodec_decode_video2(info->pCodecCtx, info->pFrame, &frameFinished, &packet);

		    		if(frameFinished) {
		                //LOGE("packet pts %llu", packet.pts);
		                // This is much different than the tutorial, sws_scale
		                // replaces img_convert, but it's not a complete drop in.
		                // This version keeps the image the same size but swaps to
		                // RGB24 format, which works perfect for PPM output.
		                int target_width = w;
		                int target_height = h;
		                img_convert_ctx = sws_getContext(info->pCodecCtx->width, info->pCodecCtx->height,
		                       info->pCodecCtx->pix_fmt,
		                       target_width, target_height, PIX_FMT_RGBA, SWS_FAST_BILINEAR,
		                       NULL, NULL, NULL);
		                if(img_convert_ctx == NULL) {
		                    LOGE("could not initialize conversion context\n");
		                    return;
		                }
		                sws_scale(img_convert_ctx, (const uint8_t* const*)info->pFrame->data, info->pFrame->linesize, 0, info->pCodecCtx->height, info->pFrameRGB->data, info->pFrameRGB->linesize);
		                if (!data)
		                	   LOGE("create data frame fail");
		                //LOGE("successful data");
		                //fill_bitmap(&info, pixels, pFrameRGB);
		                if(i==(numFrame-1))
		                {
		                	uint8_t *frameLine;
		                	  int  yy;
		                	  int rowSize=w*4;
		                	  for (yy = 0; yy < h; yy++) {
		                	      frameLine = (uint8_t *)info->pFrameRGB->data[0] + ((h-1-yy) * info->pFrameRGB->linesize[0]);
		                	      memcpy(data+yy*rowSize,frameLine,rowSize);
		                	  }
		                	  av_free_packet(&packet);
		                	  break;
		                }
		                i = i++;
		    	    }
		        }
		    }
			else
				return 1;
	
	return 0;
}

/*void drawframe(JNIEnv * env,jstring bitmap)
{
	AndroidBitmapInfo  info;
	    void*              pixels;
	    int                ret;

	    int err;
	    int i;
	    int frameFinished = 0;
	    AVPacket packet;
	    static struct SwsContext *img_convert_ctx;
	    int64_t seek_target;

	    if ((ret = AndroidBitmap_getInfo(env, bitmap, &info)) < 0) {
	        LOGE("AndroidBitmap_getInfo() failed ! error=%d", ret);
	        return;
	    }
	    LOGE("Checked on the bitmap");

	    if ((ret = AndroidBitmap_lockPixels(env, bitmap, &pixels)) < 0) {
	        LOGE("AndroidBitmap_lockPixels() failed ! error=%d", ret);
	    }
	    LOGE("Grabbed the pixels");

	    i = 0;
	    while((i==0) && (av_read_frame(pFormatCtx, &packet)>=0)) {
	  		if(packet.stream_index==videoStream) {
	            avcodec_decode_video2(pCodecCtx, pFrame, &frameFinished, &packet);

	    		if(frameFinished) {
	                LOGE("packet pts %llu", packet.pts);
	                // This is much different than the tutorial, sws_scale
	                // replaces img_convert, but it's not a complete drop in.
	                // This version keeps the image the same size but swaps to
	                // RGB24 format, which works perfect for PPM output.
	                int target_width = 320;
	                int target_height = 240;
	                img_convert_ctx = sws_getContext(pCodecCtx->width, pCodecCtx->height,
	                       pCodecCtx->pix_fmt,
	                       target_width, target_height, PIX_FMT_RGBA, SWS_FAST_BILINEAR,
	                       NULL, NULL, NULL);
	                if(img_convert_ctx == NULL) {
	                    LOGE("could not initialize conversion context\n");
	                    return;
	                }
	                sws_scale(img_convert_ctx, (const uint8_t* const*)pFrame->data, pFrame->linesize, 0, pCodecCtx->height, pFrameRGB->data, pFrameRGB->linesize);

	                // save_frame(pFrameRGB, target_width, target_height, i);
	                fill_bitmap(&info, pixels, pFrameRGB);
	                i = 1;
	    	    }
	        }
	        av_free_packet(&packet);
	    }
	    AndroidBitmap_unlockPixels(env, bitmap);
}

int seek_frame(int tsms)
{
    int64_t frame;

    frame = av_rescale(tsms,pFormatCtx->streams[videoStream]->time_base.den,pFormatCtx->streams[videoStream]->time_base.num);
    frame/=1000;
    
    if(avformat_seek_file(pFormatCtx,videoStream,0,frame,frame,AVSEEK_FLAG_FRAME)<0) {
        return 0;
    }

    avcodec_flush_buffers(pCodecCtx);

    return 1;
}*/
void deinitialize(struct AVInfo *info)
{
	 av_free(info->buffer);
	 av_free(info->pFrameRGB);

	  // Free the YUV frame
	 av_free(info->pFrame);

	 // Close the codec
	 avcodec_close(info->pCodecCtx);

	 // Close the video file
	 av_close_input_file(info->pFormatCtx);
}

