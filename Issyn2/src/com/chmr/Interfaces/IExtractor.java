package com.chmr.Interfaces;

import java.util.List;
import java.util.Map;

/**
 * Created by fury on 01.10.16.
 */
public interface IExtractor {
    Map<String,String> Extract(String content);
    Boolean Store(Map<String,String> results);
}
