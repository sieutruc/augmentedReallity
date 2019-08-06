package com.qualcomm.QCARSamples.ImageTargets;

import android.content.Context;
import android.os.Handler;
import android.os.Message;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;
import android.widget.ToggleButton;

public class GUIManager {

	// Custom views
    private View overlayView;
    private ToggleButton startButton;
    private Button clearButton;
    private Button endButton;
    private Button deleteButton;
    
    // The main application context
    private Context applicationContext;
    
    // A Handler for working with the gui from other threads
    private Handler mainActivityHandler;
    
    // Flags for our Handler
    public static final int SHOW_DELETE_BUTTON = 0;
    public static final int HIDE_DELETE_BUTTON = 1;
    public static final int TOGGLE_START_BUTTON = 2;
    public static final int DISPLAY_INFO_TOAST = 3;
    
    // Native methods to handle button clicks
    public native void nativeHome();
    public native void nativePrev();
    public native void nativeNext();
    public native void nativeEnd();
    
    
    /** Initialize the GUIManager. */
    public GUIManager(Context context)
    {
    	// Load our overlay view
    	// This view will add content on top of the camera / opengl view
    	overlayView = View.inflate(context, R.layout.interface_overlay, null);
    	
    	// Store the application context
    	applicationContext = context;
    	
    	// Create a Handler from the current thread
    	// This is the only thread that can make changes to the GUI,
    	// so we require a handler for other threads to make changes
        mainActivityHandler = new Handler() {
            @Override
            public void handleMessage(Message msg) {
                switch (msg.what) {
                    case SHOW_DELETE_BUTTON:
                        if (deleteButton != null) {
                            deleteButton.setVisibility(View.VISIBLE);
                        }
                        break;
                    case HIDE_DELETE_BUTTON:
                        if (deleteButton != null) {
                            deleteButton.setVisibility(View.INVISIBLE);
                        }
                        break;
                    case TOGGLE_START_BUTTON:
                    	if (startButton != null) {
                    		startButton.setChecked(true);
                    	}
                    	break;
                    case DISPLAY_INFO_TOAST:
                    	String text = (String) msg.obj;
                    	int duration = Toast.LENGTH_LONG;
                    	Toast toast = Toast.makeText(applicationContext, text, duration);
                    	toast.show();
                    	break;
                }
            }
        };
    }
    
    
    /** Button clicks should call corresponding native functions. */
    public void initButtons()
    {
    	if (overlayView == null)
    		return;
    	
        /*clearButton = (Button) overlayView.findViewById(R.id.clear_button);
        clearButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                nativeHome();
            }
        });
        
        endButton = (Button) overlayView.findViewById(R.id.end_button);
        endButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                nativeEnd();
            }
        });*/
        
    }
    
    
    /** Clear the button listeners. */
    public void deinitButtons()
    {
    	if (overlayView == null)
    		return;
    	
        //clearButton.setOnClickListener(null);
        
        //clearButton = null;
    }
    
    
    /** Send a message to our gui thread handler. */
    public void sendThreadSafeGUIMessage(Message message)
    {
    	mainActivityHandler.sendMessage(message);
    }
    
    
    /** Getter for the overlay view. */
    public View getOverlayView()
    {
    	return overlayView;
    }
}
