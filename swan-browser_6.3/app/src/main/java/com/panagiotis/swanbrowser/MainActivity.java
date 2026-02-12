package com.panagiotis.swanbrowser;

import android.animation.*;
import android.app.*;
import android.app.Activity;
import android.app.DialogFragment;
import android.app.Fragment;
import android.app.FragmentManager;
import android.content.*;
import android.content.ClipData;
import android.content.ClipboardManager;
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
import com.panagiotis.swanbrowser.databinding.*;
import java.io.*;
import java.text.*;
import java.util.*;
import java.util.regex.*;
import org.json.*;

public class MainActivity extends Activity {
	
	private MainBinding binding;
	private String search_engine = "";
	private String ai_agent = "";
	
	@Override
	protected void onCreate(Bundle _savedInstanceState) {
		super.onCreate(_savedInstanceState);
		binding = MainBinding.inflate(getLayoutInflater());
		setContentView(binding.getRoot());
		initialize(_savedInstanceState);
		initializeLogic();
	}
	
	private void initialize(Bundle _savedInstanceState) {
		
		binding.webview1.setWebViewClient(new WebViewClient() {
			@Override
			public void onPageStarted(WebView _param1, String _param2, Bitmap _param3) {
				final String _url = _param2;
				binding.progressbar1.setVisibility(View.VISIBLE);
				binding.progressbar1.setIndeterminate(true);
				super.onPageStarted(_param1, _param2, _param3);
			}
			
			@Override
			public void onPageFinished(WebView _param1, String _param2) {
				final String _url = _param2;
				binding.progressbar1.setVisibility(View.GONE);
				binding.progressbar1.setIndeterminate(false);
				super.onPageFinished(_param1, _param2);
			}
		});
		
		binding.button1.setOnLongClickListener(new View.OnLongClickListener() {
			@Override
			public boolean onLongClick(View _view) {
				((ClipboardManager) getSystemService(getApplicationContext().CLIPBOARD_SERVICE)).setPrimaryClip(ClipData.newPlainText("clipboard", binding.webview1.getUrl()));
				SketchwareUtil.showMessage(getApplicationContext(), "URL Copied to Clipboard");
				return true;
			}
		});
		
		binding.button1.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.linear4.setVisibility(View.VISIBLE);
				binding.edittext2.setVisibility(View.VISIBLE);
				binding.linear6.setVisibility(View.GONE);
			}
		});
		
		binding.button2.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.webview1.stopLoading();
				binding.webview1.clearCache(true);
				binding.linear10.setVisibility(View.VISIBLE);
			}
		});
		
		binding.button3.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.webview1.goBack();
			}
		});
		
		binding.button4.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.webview1.goForward();
			}
		});
		
		binding.button5.setOnLongClickListener(new View.OnLongClickListener() {
			@Override
			public boolean onLongClick(View _view) {
				binding.webview1.clearCache(true);
				binding.webview1.stopLoading();
				binding.webview1.loadUrl(binding.webview1.getUrl());
				return true;
			}
		});
		
		binding.button5.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.webview1.stopLoading();
				binding.webview1.clearCache(true);
				binding.webview1.loadUrl("https://".concat(binding.edittext1.getText().toString()));
				binding.linear10.setVisibility(View.GONE);
				SketchwareUtil.hideKeyboard(getApplicationContext());
			}
		});
		
		binding.textview1.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.linear4.setVisibility(View.GONE);
			}
		});
		
		binding.textview2.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.edittext2.setVisibility(View.GONE);
				binding.linear6.setVisibility(View.VISIBLE);
			}
		});
		
		binding.textview4.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.linear4.setVisibility(View.GONE);
				binding.linear10.setVisibility(View.GONE);
				binding.webview1.loadUrl("https://panagiotis-katziotis.github.io/upadateswanbrowser/");
			}
		});
		
		binding.circleimageview1.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				search_engine = "https://www.google.com/search?q=";
			}
		});
		
		binding.circleimageview2.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				search_engine = "https://search.brave.com/search?q=";
			}
		});
		
		binding.circleimageview3.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				search_engine = "https://duckduckgo.com/?ia=web&origin=funnel_home_website&t=h_&hps=1&start=1&q=";
			}
		});
		
		binding.circleimageview4.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				search_engine = "https://opnxng.com/search?q=";
			}
		});
		
		binding.circleimageview5.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				search_engine = "https://www.ecosia.org/search? method=index&q=";
			}
		});
		
		binding.circleimageview6.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.linear10.setBackgroundColor(0xFF000000);
				binding.imageview1.setVisibility(View.VISIBLE);
				binding.edittext3.setHintTextColor(0xFFFFFFFF);
				binding.edittext3.setTextColor(0xFFFFFFFF);
				binding.textview8.setTextColor(0xFFFFFFFF);
				binding.textview9.setTextColor(0xFFFFFFFF);
			}
		});
		
		binding.circleimageview7.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.linear10.setBackgroundColor(0xFF424242);
				binding.imageview1.setVisibility(View.INVISIBLE);
				binding.edittext3.setHintTextColor(0xFFFFFFFF);
				binding.edittext3.setTextColor(0xFFFFFFFF);
				binding.textview8.setTextColor(0xFFFFFFFF);
				binding.textview9.setTextColor(0xFFFFFFFF);
			}
		});
		
		binding.circleimageview8.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.linear10.setBackgroundColor(0xFFFFF8E1);
				binding.edittext3.setHintTextColor(0xFF000000);
				binding.edittext3.setTextColor(0xFF000000);
				binding.textview8.setTextColor(0xFF000000);
				binding.textview9.setTextColor(0xFF000000);
				binding.imageview1.setVisibility(View.INVISIBLE);
			}
		});
		
		binding.circleimageview11.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				ai_agent = "https://chatgpt.com";
			}
		});
		
		binding.circleimageview12.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				ai_agent = "https://claude.ai";
			}
		});
		
		binding.circleimageview13.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				ai_agent = "https://perplexity.ai";
			}
		});
		
		binding.circleimageview14.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				ai_agent = "https://copilot.microsoft.com";
			}
		});
		
		binding.circleimageview15.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				ai_agent = "https://gemini.google.com";
			}
		});
		
		binding.imageview1.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.linear10.setVisibility(View.GONE);
				binding.webview1.loadUrl("https://panagiotis-katziotis.github.io/swanproject");
			}
		});
		
		binding.textview8.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.webview1.stopLoading();
				binding.webview1.clearCache(true);
				binding.webview1.loadUrl(search_engine.concat(binding.edittext3.getText().toString()));
				binding.linear10.setVisibility(View.GONE);
			}
		});
		
		binding.textview9.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View _view) {
				binding.webview1.stopLoading();
				binding.webview1.clearCache(true);
				binding.linear10.setVisibility(View.GONE);
				binding.webview1.loadUrl(ai_agent);
			}
		});
	}
	
	private void initializeLogic() {
		search_engine = "https://duckduckgo.com/?ia=web&origin=funnel_home_website&t=h_&hps=1&start=1&q=";
		ai_agent = "https://chatgpt.com";
		binding.linear4.setVisibility(View.GONE);
		binding.linear6.setVisibility(View.GONE);
		WebView webview1 = (WebView) findViewById(R.id.webview1);
		webview1.getSettings().setJavaScriptEnabled(true);
		webview1.getSettings().setAllowFileAccess(true);
		webview1.getSettings().setAllowContentAccess(true);
		webview1.getSettings().setDomStorageEnabled(true);
		
		webview1.setDownloadListener(new DownloadListener() {
			@Override
			public void onDownloadStart(
			String url,
			String userAgent,
			String contentDisposition,
			String mimeType,
			long contentLength) {
				
				DownloadManager.Request request =
				new DownloadManager.Request(Uri.parse(url));
				
				String fileName =
				URLUtil.guessFileName(url, contentDisposition, mimeType);
				
				request.setTitle(fileName);
				request.setDescription("Downloading...");
				request.setMimeType(mimeType);
				request.addRequestHeader("User-Agent", userAgent);
				
				request.setNotificationVisibility(
				DownloadManager.Request.VISIBILITY_VISIBLE_NOTIFY_COMPLETED);
				
				request.setDestinationInExternalPublicDir(
				Environment.DIRECTORY_DOWNLOADS,
				fileName
				);
				
				DownloadManager dm =
				(DownloadManager) getSystemService(DOWNLOAD_SERVICE);
				
				dm.enqueue(request);
				
				Toast.makeText(getApplicationContext(),
				"Λήψη αρχείου...", Toast.LENGTH_SHORT).show();
			}
		});
	}
	
}