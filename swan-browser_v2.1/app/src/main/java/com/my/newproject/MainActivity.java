package com.my.newproject;

import android.Manifest;
import android.animation.*;
import android.app.*;
import android.content.*;
import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.res.*;
import android.graphics.*;
import android.graphics.drawable.*;
import android.media.*;
import android.net.*;
import android.os.*;
import android.text.*;
import android.text.style.*;
import android.util.*;
import android.view.*;
import android.view.View;
import android.view.View.*;
import android.view.animation.*;
import android.webkit.*;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.*;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import androidx.annotation.*;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import java.io.*;
import java.text.*;
import java.util.*;
import java.util.regex.*;
import org.json.*;

public class MainActivity extends AppCompatActivity {
	
	public final int REQ_CD_TABS = 101;
	
	private LinearLayout linear1;
	private LinearLayout linear2;
	private ProgressBar progressbar1;
	private WebView webview1;
	private Button button7;
	private Button button1;
	private Button button2;
	private Button button3;
	private EditText edittext1;
	private Button button4;
	private EditText edittext2;
	private Button button5;
	private Button button6;
	
	private Intent tabs = new Intent(Intent.ACTION_GET_CONTENT);
	
	@Override
	protected void onCreate(Bundle _savedInstanceState) {
		super.onCreate(_savedInstanceState);
		setContentView(R.layout.main);
		initialize(_savedInstanceState);
		
		if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE) == PackageManager.PERMISSION_DENIED
		|| ContextCompat.checkSelfPermission(this, Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_DENIED) {
			ActivityCompat.requestPermissions(this, new String[] {Manifest.permission.READ_EXTERNAL_STORAGE, Manifest.permission.WRITE_EXTERNAL_STORAGE}, 1000);
		} else {
			initializeLogic();
		}
	}
	
	@Override
	public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
		super.onRequestPermissionsResult(requestCode, permissions, grantResults);
		if (requestCode == 1000) {
			initializeLogic();
		}
	}
	
	private void initialize(Bundle _savedInstanceState) {
		linear1 = findViewById(R.id.linear1);
		linear2 = findViewById(R.id.linear2);
		progressbar1 = findViewById(R.id.progressbar1);
		webview1 = findViewById(R.id.webview1);
		webview1.getSettings().setJavaScriptEnabled(true);
		webview1.getSettings().setSupportZoom(true);
		button7 = findViewById(R.id.button7);
		button1 = findViewById(R.id.button1);
		button2 = findViewById(R.id.button2);
		button3 = findViewById(R.id.button3);
		edittext1 = findViewById(R.id.edittext1);
		button4 = findViewById(R.id.button4);
		edittext2 = findViewById(R.id.edittext2);
		button5 = findViewById(R.id.button5);
		button6 = findViewById(R.id.button6);
		tabs.setType("text/*");
		tabs.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
		
		webview1.setWebViewClient(new WebViewClient() {
			@Override
			public void onPageStarted(WebView _param1, String _param2, Bitmap _param3) {
				final String _url = _param2;
				progressbar1.setIndeterminate(true);
				super.onPageStarted(_param1, _param2, _param3);
			}
			
			@Override
			public void onPageFinished(WebView _param1, String _param2) {
				final String _url = _param2;
				progressbar1.setIndeterminate(false);
				super.onPageFinished(_param1, _param2);
			}
		});
		
		button7.setOnLongClickListener(new View.OnLongClickListener() {
			@Override
			public boolean onLongClick(View _view) {
				webview1.loadUrl(webview1.getUrl());
				return true;
			}
		});
		
		button7.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				((ClipboardManager) getSystemService(getApplicationContext().CLIPBOARD_SERVICE)).setPrimaryClip(ClipData.newPlainText("clipboard", webview1.getUrl()));
				linear2.setVisibility(View.VISIBLE);
				edittext2.setVisibility(View.VISIBLE);
				button6.setVisibility(View.VISIBLE);
				button5.setVisibility(View.VISIBLE);
				
				
			}
		});
		
		button1.setOnLongClickListener(new View.OnLongClickListener() {
			@Override
			public boolean onLongClick(View _view) {
				webview1.loadUrl("https://chatgpt.com");
				return true;
			}
		});
		
		button1.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				webview1.loadUrl("https://www.google.com");
			}
		});
		
		button2.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				webview1.goBack();
			}
		});
		
		button3.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				webview1.goForward();
			}
		});
		
		button4.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				webview1.loadUrl("https://".concat(edittext1.getText().toString()));
			}
		});
		
		button5.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				FileUtil.writeFile(FileUtil.getExternalStorageDir().concat("/swanbrowserdata/".concat("tabs.txt")), edittext2.getText().toString());
				SketchwareUtil.showMessage(getApplicationContext(), "Save Complete ");
			}
		});
		
		button6.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				linear2.setVisibility(View.GONE);
				edittext2.setVisibility(View.GONE);
				button5.setVisibility(View.GONE);
				
				button6.setVisibility(View.GONE);
				SketchwareUtil.showMessage(getApplicationContext(), "Saved at /swanbrowserdata/tabs.txt");
			}
		});
	}
	
	private void initializeLogic() {
		button5.setVisibility(View.GONE);
		
		edittext2.setVisibility(View.GONE);
		linear2.setVisibility(View.GONE);
		button6.setVisibility(View.GONE);
		webview1.loadUrl("https://www.google.com");
	}
	
	
	@Deprecated
	public void showMessage(String _s) {
		Toast.makeText(getApplicationContext(), _s, Toast.LENGTH_SHORT).show();
	}
	
	@Deprecated
	public int getLocationX(View _v) {
		int _location[] = new int[2];
		_v.getLocationInWindow(_location);
		return _location[0];
	}
	
	@Deprecated
	public int getLocationY(View _v) {
		int _location[] = new int[2];
		_v.getLocationInWindow(_location);
		return _location[1];
	}
	
	@Deprecated
	public int getRandom(int _min, int _max) {
		Random random = new Random();
		return random.nextInt(_max - _min + 1) + _min;
	}
	
	@Deprecated
	public ArrayList<Double> getCheckedItemPositionsToArray(ListView _list) {
		ArrayList<Double> _result = new ArrayList<Double>();
		SparseBooleanArray _arr = _list.getCheckedItemPositions();
		for (int _iIdx = 0; _iIdx < _arr.size(); _iIdx++) {
			if (_arr.valueAt(_iIdx))
			_result.add((double)_arr.keyAt(_iIdx));
		}
		return _result;
	}
	
	@Deprecated
	public float getDip(int _input) {
		return TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, _input, getResources().getDisplayMetrics());
	}
	
	@Deprecated
	public int getDisplayWidthPixels() {
		return getResources().getDisplayMetrics().widthPixels;
	}
	
	@Deprecated
	public int getDisplayHeightPixels() {
		return getResources().getDisplayMetrics().heightPixels;
	}
}