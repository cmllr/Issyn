package com.issyn.Data;

import java.net.URL;
import java.util.Date;
import java.util.Map;

/**
 * Created by fury on 24.01.2016.
 */
public class Index {
    public URL Page;
    public String Title;
    public String[] Hyperlinks;
    public Map<String,String> Meta;
    public String[] JS;
    public String[] Keywords;
    public String CMS;
    public Date Indexed;
    public Date LastIndexed;
    public URL Parent;
    public Index(URL page, String[] hyperlinks,Map<String,String> meta,String cms,String[] keywords,String[] js,String title,URL parent){
        this.Page = page;
        this.Hyperlinks = hyperlinks;
        this.Meta = meta;
        this.CMS = cms;
        this.Keywords = keywords;
        this.JS = js;
        this.Parent = parent;
        this.Indexed = new Date();
        this.LastIndexed = this.Indexed;
        this.Title = title;
    }
}
