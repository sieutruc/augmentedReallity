package com.qualcomm.QCARSamples.ImageTargets;

import java.util.List;
import java.util.ArrayList;

public class WebPage {

	List<MediaObject> objects;
	List<Marker> markers;
	
	public WebPage()
	{
		objects = new ArrayList<MediaObject>();
		markers = new ArrayList<Marker>();
	}
	
	public void add(MediaObject obj)
	{
		objects.add(obj);
	}
	
	public void add(Marker m)
	{
		markers.add(m);
	}
	
	public int numObjects()
	{
		return objects.size();
	}
	
	public int numMarkers()
	{
		return markers.size();
	}
	
	public MediaObject get(int i)
	{
		return objects.get(i); 
	}
	
	public Marker getMarker(int i)
	{
		return markers.get(i);
	}
	
	public WebPage copy()
	{
		WebPage page = new WebPage();
		
		int size = objects.size();
		page.objects = new ArrayList<MediaObject>(size);
		for(int i = 0; i < size; i++)
			page.objects.add(objects.get(i));
		
		int sizeMk = markers.size();
		page.markers = new ArrayList<Marker>(sizeMk);
		for(int i = 0; i < sizeMk; i++)
			page.markers.add(markers.get(i));
		
		return page;
	}
	
	public void clear()
	{
		objects.clear();
		markers.clear();
	}
}