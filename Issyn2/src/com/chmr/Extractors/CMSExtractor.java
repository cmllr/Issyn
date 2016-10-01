package com.chmr.Extractors;

import com.chmr.Hypervisor;
import com.chmr.Interfaces.IExtractor;

import java.net.URL;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Created by fury on 01.10.16.
 */
public class CMSExtractor implements IExtractor {
    private String cms = "";
    private URL target;
    @Override
    public Map<String,String> Extract(String content,URL target) {
        this.target = target;
        Map<String,String> meta = new MetaExtractor().Extract(content,target);
        for(Map.Entry<String,String> tag :  meta.entrySet()){
            if (tag.getKey().toLowerCase().equals("generator")){
                this.cms  = tag.getValue();
            }
        }
        return null;
    }
    public Boolean Store(Map<String,String> result){
        Hypervisor.Output(String.format("CMS is \"%s\"",this.cms));
        return true;
    }

    public Boolean IsResultCrawlable(){
        return false;
    }
}
