package com.qualcomm.QCARSamples.ImageTargets;

import java.util.ArrayList;
import java.util.List;

import android.sax.Element;
import android.sax.EndElementListener;
import android.sax.EndTextElementListener;
import android.sax.RootElement;
import android.util.Xml;

public class AndroidSaxFeedParser extends BaseFeedParser {

	static final String RSS = "rss";
	public AndroidSaxFeedParser(String feedUrl) {
		super(feedUrl);
	}

	public List<WebPage> parse() {
		
		RootElement paper = new RootElement(PAPER);
		final List<WebPage> web = new ArrayList<WebPage>();
		
		final WebPage currentPage = new WebPage();
		
		Element pages = paper.getChild(PAGES);
		Element page = pages.getChild(PAGE);
		page.setEndElementListener(new EndElementListener(){
			public void end() {
				web.add(currentPage.copy());
				currentPage.clear();
			}
		});
		
		final MediaObject[] objects = {new MediaObject(), new MediaObject(), new MediaObject(), new Audio(), new MediaObject()};
		final MediaObject tmp = new MediaObject();
		final Marker currentMarker = new Marker();
		
		Element object = page.getChild(MEDIAOBJECT);
		Element type = object.getChild(TYPE);
		
		type.setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				MediaObject currentObject;
				
				if(body.equalsIgnoreCase("NewsPaperPicture"))
					tmp.setType(0);
				if(body.equalsIgnoreCase("NewsPaperPictureCollection"))
					tmp.setType(1);
				if(body.equalsIgnoreCase("NewsPaperVideo"))
					tmp.setType(2);
				if(body.equalsIgnoreCase("NewsPaperAudio"))
					tmp.setType(3);
				if(body.equalsIgnoreCase("md2model"))
					tmp.setType(4);
				
				currentObject = objects[tmp.getType()];
				currentObject.setType(tmp.getType());
				currentObject.clearUris();
			}
		});
		
		object.setEndElementListener(new EndElementListener(){
			public void end() {
				MediaObject currentObject = objects[tmp.getType()];
				currentPage.add(currentObject.copy());
			}
		});
		object.getChild(URI).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				MediaObject currentObject = objects[tmp.getType()];
				currentObject.addUri(body);
			}
		});
		object.getChild(X).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				MediaObject currentObject = objects[tmp.getType()];
				currentObject.setX(Integer.valueOf(body));
			}
		});
		object.getChild(Y).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				MediaObject currentObject = objects[tmp.getType()];
				currentObject.setY(Integer.valueOf(body));
			}
		});
		object.getChild(WIDTH).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				MediaObject currentObject = objects[tmp.getType()];
				currentObject.setWidth(Integer.valueOf(body));
			}
		});
		object.getChild(HEIGHT).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				MediaObject currentObject = objects[tmp.getType()];
				currentObject.setHeight(Integer.valueOf(body));
			}
		});
		object.getChild(MARKER).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				MediaObject currentObject = objects[tmp.getType()];
				currentObject.setMarker(body);
			}
		});
		
		Element marker = page.getChild(MARKER);
		
		marker.setEndElementListener(new EndElementListener(){
			public void end() {
				currentMarker.setPage(web.size());
				currentPage.add(currentMarker.copy());
			}
		});
		marker.getChild(X).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				currentMarker.setX(Float.valueOf(body));
			}
		});
		marker.getChild(Y).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				currentMarker.setY(Float.valueOf(body));
			}
		});
		marker.getChild(WIDTH).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				currentMarker.setWidth(Float.valueOf(body));
			}
		});
		marker.getChild(HEIGHT).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				currentMarker.setHeight(Float.valueOf(body));
			}
		});
		marker.getChild(NAME).setEndTextElementListener(new EndTextElementListener(){
			public void end(String body) {
				currentMarker.setName(body);
			}
		});
		
		try {
			Xml.parse(this.getInputStream(), Xml.Encoding.UTF_8, paper.getContentHandler());
		} catch (Exception e) {
			throw new RuntimeException(e);
		}
		return web;
	}
}
