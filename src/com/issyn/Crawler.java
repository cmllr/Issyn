package com.issyn;

import com.issyn.Data.Index;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Created by fury on 24.01.2016.
 */
class Crawler {
    private URL target;
    public Crawler(URL target){
        this.target = target;
    }
    public Index GetIndex(URL target){
        if (Hypervisor.indizes == null)
        {
            return null;
        }
        for(int i = 0; i < Hypervisor.indizes.length;i++){
            if (Hypervisor.indizes[i].Page.equals(target)){
                return Hypervisor.indizes[i];
            }
        }
        return null;
    }
    public Index Crawl(URL parent) throws MalformedURLException {
        if (this.GetIndex(this.target) != null) {
            return this.GetIndex(this.target);
        }
        if (!this.IsCrawable(this.target)){
            return null;
        }
        String content = Downloader.DownloadSite(this.target);
        if (content.equals("")){
            return null;
        }
        String[] allHyperlinks = this.ExtractHyperlinks(content);
        String[] ownHyperlinks = this.ExtractOwnHyperlinks(allHyperlinks);
        System.out.println(String.format("Found %s Hyperlinks hosted on %s",ownHyperlinks.length,this.target));
        Map<String,String> meta = this.ExtractMetaTags(content);
        String[] js = this.ExtractJSFrameworks(content);
        return new Index(target,ownHyperlinks,meta,this.ExtractCMS(meta),this.ExtractKeywords(meta),js,this.GetTitle(content),parent,content);
    }
    private Boolean IsCrawable(URL target){
        //TODO: Better solution
        return !target.toString().endsWith("png") &&
                !target.toString().endsWith(".js");
    }
    private String[] ExtractHyperlinks(String content){
        Pattern href = Pattern.compile("<\\s{0,}a.{0,}href\\s{0,}=\\s{0,}(\"|')(?<href>[^(\"|')]+)(\"|')",Pattern.MULTILINE | Pattern.CASE_INSENSITIVE);
        Matcher m = href.matcher(content);
        List<String> matches = new ArrayList();
        while(m.find()){
            String match = m.group("href");
            if (!matches.contains(match) && !match.endsWith("#") && !match.contains("#") ){
                matches.add(matches.size(),match);
            }
        }
        return matches.toArray(new String[]{});
    }
    private String[] ExtractOwnHyperlinks(String[] all) throws  MalformedURLException{
        List<String> matches = new ArrayList();
        for(int i = 0; i < all.length;i++) {
            URL current = null;

                try {
                    current = new URL(all[i]);
                } catch (MalformedURLException ex) {
                    try{
                        current = new URL(target, all[i].replace("./", ""));
                    }
                    catch(MalformedURLException mex){
                        //Cannot parse
                    }
                }
                //either same host or subdomain
                try{
                    if (current != null && target.getHost().equals(current.getHost()) || current.getHost().contains(target.getHost())) {
                        matches.add(current.toString());
                    }
                }catch(Exception e){
                    e.printStackTrace();
                }
        }
        return matches.toArray(new String[]{});
    }
    private Map<String,String> ExtractMetaTags(String content){
        Map<String, String> map = new HashMap<String, String>();
        Pattern meta = Pattern.compile("<\\s{0,}meta\\s{1,}name\\s{0,}=\\s{0,}(\\\"|')(?<name>[^(\\\"|')]+)(\\\"|')\\s{1,}content\\s{0,}=\\s{0,}(\\\"|')(?<content>[^(\\\"|')]+)(\\\"|')",Pattern.MULTILINE | Pattern.CASE_INSENSITIVE );
        Matcher m = meta.matcher(content);
        while(m.find()){
            String metaname = m.group("name");
            String metacontent = m.group("content");
            map.put(metaname,metacontent);
        }
        return map;
    }
    private String ExtractCMS(Map<String, String> meta){
        for(Map.Entry<String,String> tag :  meta.entrySet()){
            if (tag.getKey().toLowerCase().equals("generator")){
                return tag.getValue();
            }
        }
        return "";
    }
    private String[] ExtractKeywords(Map<String, String> meta){
        for(Map.Entry<String,String> tag :  meta.entrySet()){
            if (tag.getKey().toLowerCase().equals("keywords")){
                return tag.getValue().split(",");
            }
        }
        return new String[]{};
    }
    private String[] ExtractJSFrameworks(String content){
        List<String> matches = new ArrayList();
        Pattern meta = Pattern.compile("<\\s{0,}script.{0,}src\\s{0,}=\\s{0,}(\\\"|')(?<script>[^(\\\"|')]+)(\\\"|')",Pattern.MULTILINE | Pattern.CASE_INSENSITIVE );
        Matcher m = meta.matcher(content);
        while(m.find()){
            String script = m.group("script");
            matches.add(script);
        }
        return matches.toArray(new String[]{});
    }
    private String GetTitle(String content){
        Pattern title = Pattern.compile("<\\s{0,}title\\s{0,}>(?<title>[^\\<]*)",Pattern.CASE_INSENSITIVE);
        Matcher m = title.matcher(content);
        while(m.find()){
            return m.group("title");
        }
        return "";
    }
}
