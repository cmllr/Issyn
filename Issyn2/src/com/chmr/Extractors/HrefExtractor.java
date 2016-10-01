package com.chmr.Extractors;

import com.chmr.Configuration;
import com.chmr.Hypervisor;
import com.chmr.Interfaces.IExtractor;
import com.chmr.Main;
import com.sun.org.apache.xerces.internal.impl.xpath.regex.Match;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Created by fury on 01.10.16.
 */
public class HrefExtractor implements IExtractor {

    private URL target;
    @Override
    public Map<String,String> Extract(String content,URL target) throws MalformedURLException {
        this.target = target;
        Map<String, String> map = new HashMap<String, String>();
        if (!target.getHost().equals(Configuration.HOST)){
            return map;
        }
        Pattern href = Pattern.compile("<\\s{0,}a.{0,}href\\s{0,}=\\s{0,}(\"|')(?<href>[^(\"|')]+)(\"|')",Pattern.MULTILINE | Pattern.CASE_INSENSITIVE);
        Matcher m = href.matcher(content);
        int index = 0;
        while(m.find()){
            String match = m.group("href");
            String correctedMatch = this.CorrectLink(match);
            if (!correctedMatch.equals(match)){
                match = correctedMatch;
            }
            if (!match.equals("")){
                int key = index++;
                map.put("" + key,match);

                Main.allLinks.add(match);
            }
            else{
                Hypervisor.Output("1 Empty link removed");
            }
        }
        return map;
    }
    public Boolean Store(Map<String,String> result){
        for(Map.Entry<String,String> tuple :  result.entrySet()){
            Hypervisor.Output(String.format("\"%s\" => \"%s\"",tuple.getKey(),tuple.getValue()));
        }
        return true;
    }
    //link Corrections
    private String CorrectLink(String link) throws MalformedURLException {
        String correction = link;
        if (link.equals(".")){
            correction = this.target.getProtocol() + "://" + this.target.getHost() + this.target.getPath();
        }
        else if (link.startsWith("?")) {
            correction = link.replace("?",this.target.getProtocol() + "://" + this.target.getHost() + this.target.getPath() + "?");
        }
        else if (link.equals("../")){
            //no upwarts traveling
            correction = "";
        }else if (link.startsWith("#")){
            //Anchors
            correction = "";
        }else{
            Pattern href = Pattern.compile("^(?<firstChar>(^[a-z0-9]))",Pattern.CASE_INSENSITIVE);
            Matcher m = href.matcher(correction);
            Boolean found = m.find();
            try{
                URL reference = new URL(link);
                if (found && reference.getHost().equals(this.target.getHost())){
                    correction = this.target.toString() + link;
                }else{
                    correction = "";
                }
            }catch(MalformedURLException ex){
                if (!link.contains("://")){
                    correction = this.target.toString() + link;
                }else{
                    correction = "";
                }
            }
        }
        return correction;
    }

    public Boolean IsResultCrawlable(){
        return true;
    }
}