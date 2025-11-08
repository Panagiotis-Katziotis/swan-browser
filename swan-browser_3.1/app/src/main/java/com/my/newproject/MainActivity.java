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
import android.widget.ImageView;
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
	private LinearLayout linear4;
	private WebView webview1;
	private LinearLayout linear3;
	private Button button7;
	private Button button9;
	private Button button2;
	private Button button3;
	private EditText edittext1;
	private Button button4;
	private EditText edittext2;
	private Button button5;
	private Button button6;
	private ImageView imageview1;
	private EditText edittext3;
	private Button button8;
	
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
		linear4 = findViewById(R.id.linear4);
		webview1 = findViewById(R.id.webview1);
		webview1.getSettings().setJavaScriptEnabled(true);
		webview1.getSettings().setSupportZoom(true);
		linear3 = findViewById(R.id.linear3);
		button7 = findViewById(R.id.button7);
		button9 = findViewById(R.id.button9);
		button2 = findViewById(R.id.button2);
		button3 = findViewById(R.id.button3);
		edittext1 = findViewById(R.id.edittext1);
		button4 = findViewById(R.id.button4);
		edittext2 = findViewById(R.id.edittext2);
		button5 = findViewById(R.id.button5);
		button6 = findViewById(R.id.button6);
		imageview1 = findViewById(R.id.imageview1);
		edittext3 = findViewById(R.id.edittext3);
		button8 = findViewById(R.id.button8);
		tabs.setType("text/*");
		tabs.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
		
		webview1.setWebViewClient(new WebViewClient() {
			@Override
			public void onPageStarted(WebView _param1, String _param2, Bitmap _param3) {
				final String _url = _param2;
				progressbar1.setIndeterminate(true);
				progressbar1.setVisibility(View.VISIBLE);
				super.onPageStarted(_param1, _param2, _param3);
			}
			
			@Override
			public void onPageFinished(WebView _param1, String _param2) {
				final String _url = _param2;
				progressbar1.setIndeterminate(false);
				progressbar1.setVisibility(View.GONE);
				super.onPageFinished(_param1, _param2);
			}
		});
		
		button7.setOnLongClickListener(new View.OnLongClickListener() {
			@Override
			public boolean onLongClick(View _view) {
				((ClipboardManager) getSystemService(getApplicationContext().CLIPBOARD_SERVICE)).setPrimaryClip(ClipData.newPlainText("clipboard", webview1.getUrl()));
				SketchwareUtil.showMessage(getApplicationContext(), "URL Copied to Clipboard ");
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
				
				edittext3.setText(FileUtil.readFile("/swanbrowserdata/tabs.txt"));
			}
		});
		
		button9.setOnLongClickListener(new View.OnLongClickListener() {
			@Override
			public boolean onLongClick(View _view) {
				webview1.loadUrl("https://chatgpt.com");
				linear4.setVisibility(View.GONE);
				return true;
			}
		});
		
		button9.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				linear4.setVisibility(View.GONE);
				webview1.loadUrl("https://start.duckduckgo.com");
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
		
		button4.setOnLongClickListener(new View.OnLongClickListener() {
			@Override
			public boolean onLongClick(View _view) {
				webview1.loadUrl(webview1.getUrl());
				return true;
			}
		});
		
		button4.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				webview1.loadUrl("https://".concat(edittext1.getText().toString()));
				linear4.setVisibility(View.GONE);
				SketchwareUtil.hideKeyboard(getApplicationContext());
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
		
		imageview1.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				button8.setVisibility(View.GONE);
				linear4.setVisibility(View.GONE);
				edittext3.setVisibility(View.GONE);
				webview1.loadUrl("https://swanproject.my.canva.site");
			}
		});
		
		button8.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				webview1.loadUrl("https://duckduckgo.com/?ia=web&origin=funnel_home_website&t=h_&hps=1&start=1&q=".concat(edittext3.getText().toString()));
				linear4.setVisibility(View.GONE);
				edittext3.setVisibility(View.GONE);
				button8.setVisibility(View.GONE);
				SketchwareUtil.hideKeyboard(getApplicationContext());
			}
		});
	}
	
	private void initializeLogic() {
		button5.setVisibility(View.GONE);
		
		edittext2.setVisibility(View.GONE);
		linear2.setVisibility(View.GONE);
		button6.setVisibility(View.GONE);
		linear4.setVisibility(View.VISIBLE);
		linear3.setVisibility(View.GONE);
		edittext3.setVisibility(View.VISIBLE);
		button8.setVisibility(View.VISIBLE);
		_download("/Download");
	}
	
	@Override
	protected void onActivityResult(int _requestCode, int _resultCode, Intent _data) {
		super.onActivityResult(_requestCode, _resultCode, _data);
		
		switch (_requestCode) {
			case REQ_CD_TABS:
			if (_resultCode == Activity.RESULT_OK) {
				ArrayList<String> _filePath = new ArrayList<>();
				if (_data != null) {
					if (_data.getClipData() != null) {
						for (int _index = 0; _index < _data.getClipData().getItemCount(); _index++) {
							ClipData.Item _item = _data.getClipData().getItemAt(_index);
							_filePath.add(FileUtil.convertUriToFilePath(getApplicationContext(), _item.getUri()));
						}
					}
					else {
						_filePath.add(FileUtil.convertUriToFilePath(getApplicationContext(), _data.getData()));
					}
				}
				
			}
			else {
				
			}
			break;
			default:
			break;
		}
	}
	
	public void _download(final String _path) {
		
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