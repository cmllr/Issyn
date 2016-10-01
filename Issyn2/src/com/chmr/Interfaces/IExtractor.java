package com.chmr.Interfaces;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.List;
import java.util.Map;

/**
 * Created by fury on 01.10.16.
 */
public interface IExtractor {
    Map<String,String> Extract(String content,URL target) throws MalformedURLException;
    Boolean Store(Map<String,String> results);
    Boolean IsResultCrawlable();
}
