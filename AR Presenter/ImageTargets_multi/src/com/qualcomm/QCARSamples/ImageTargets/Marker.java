package com.qualcomm.QCARSamples.ImageTargets;

public class Marker {
	protected float x;
	protected float y;
	protected float width;
	protected float height;
	protected String name;
	protected int page;
	
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
	
	public float getWidth() {
		return width;
	}
	public void setWidth(float width) {
		this.width = width;
	}
	
	public float getHeight() {
		return height;
	}
	public void setHeight(float height) {
		this.height = height;
	}
	
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	
	public int getPage() {
		return page;
	}
	public void setPage(int page) {
		this.page = page;
	}
	
	public Marker copy()
	{
		Marker obj = new Marker();
		obj.x = x;
		obj.y = y;
		obj.width = width;
		obj.height = height;
		obj.name = name;
		obj.page = page;
		return obj;
	}
}