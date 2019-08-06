package com.qualcomm.QCARSamples.ImageTargets;
import java.io.*;
import java.net.MalformedURLException;
import java.net.URL;

public abstract class BaseFeedParser implements FeedParser {

	// names of the XML tags
	static final String NUMPAGES = "numpages";
	static final String TYPE = "type";
	static final String URI = "uri";
	static final String X = "x";
	static final String Y = "y";
	static final String WIDTH = "width";
	static final String HEIGHT = "height";
	static final String MARKER = "marker";
	static final String NAME = "name";
	static final String MEDIAOBJECT = "object";
	static final String PAPER = "newspaper";
	static final String PAGES = "pages";
	static final String PAGE = "page";
	
	private final String feedUrl;

	protected BaseFeedParser(String feedUrl){
		try {
			this.feedUrl = feedUrl;
		} catch (Exception e) {
			throw new RuntimeException(e);
		}
	}

	protected InputStream getInputStream() {
		try {
			FileInputStream stream = new FileInputStream(feedUrl);
			return stream;
		} catch (IOException e) {
			throw new RuntimeException(e);
		}
	}
}