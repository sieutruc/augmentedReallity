package com.qualcomm.QCARSamples.ImageTargets;

import android.media.MediaPlayer;

import java.io.IOException;
import java.util.List;
import java.util.ArrayList;

public class MediaObject {
	protected float x;
	protected float y;
	protected int width;
	protected int height;
	protected List<String> uris;
	protected int type;
	protected int texId;
	protected String marker;
	
	public MediaObject()
	{
		uris = new ArrayList<String>();
		texId = 0;
	}
	
	public void initOtherData()
	{}
	
	public List<String> getListUri()
	{
		return uris;
	}
	public String getUri(int i)
	{
		return uris.get(i);
	}
	public int getNumUri()
	{
		return uris.size();
	}
	public void addUri(String uri) {
		uris.add(uri);
	}
	public void setUri(int i, String s)
	{
		uris.set(i, s);
	}
	public void clearUris()
	{
		uris.clear();
	}
	
	public float getX() {
		return x;
	}
	public void setX(float x) {
		this.x = x;
	}
	
	public float getY() {
		return y;
	}
	public void setY(float y) {
		this.y = y;
	}
	
	public int getWidth() {
		return width;
	}
	public void setWidth(int width) {
		this.width = width;
	}
	
	public int getHeight() {
		return height;
	}
	public void setHeight(int height) {
		this.height = height;
	}
	
	public int getType()
	{
		return type;
	}	
	public void setType(int type)
	{
		this.type = type;
	}
	
	public int getTexId()
	{
		return texId;
	}	
	public void setTexId(int texId)
	{
		this.texId = texId;
	}
	
	public String getMarker()
	{
		return marker;
	}
	public void setMarker(String m)
	{
		marker = m;
	}
	
	public void start() 
	{}
	public void pause()
	{}
	
	public void copyMembersTo(MediaObject obj)
	{
		obj.uris = new ArrayList<String>(uris.size());
		for(int i = 0; i < uris.size(); i++)
			obj.uris.add(uris.get(i));
		obj.x = x;
		obj.y = y;
		obj.width = width;
		obj.height = height;
		obj.type = type;
		obj.texId = texId;
		obj.marker = marker;
	}
	
	public MediaObject copy()
	{
		MediaObject obj = new MediaObject();
		copyMembersTo(obj);
		return obj;
	}	
}

class Audio extends MediaObject {
	protected MediaPlayer player;
	boolean initialized = false;
	
	@Override
	public void initOtherData()
	{
		player = new MediaPlayer();
		try {
			player.setDataSource(getUri(0));
		} 
		catch (IllegalArgumentException e) {
			e.printStackTrace();
		} 
		catch (IOException e) {
			e.printStackTrace();
		}
	}

	@Override
	public MediaObject copy()
	{
		Audio audio = new Audio();
		copyMembersTo(audio);
		audio.player = player;
		return audio;
	}
	
	@Override 
	public void start()
	{
		try {
			if(!initialized)
			{
				player.prepare();
				initialized = true;
			}
		} 
		catch (IOException e) {
			e.printStackTrace();
		}
		player.start();
	}
	
	@Override
	public void pause()
	{
		player.pause();
	}
}