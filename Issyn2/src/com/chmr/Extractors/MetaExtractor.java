package com.chmr.Extractors;

import com.chmr.Hypervisor;
import com.chmr.Interfaces.IExtractor;

import java.util.HashMap;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Created by fury on 01.10.16.
 */
public class MetaExtractor implements IExtractor {
    @Override
    public Map<String,String> Extract(String content) {
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
    public Boolean Store(Map<String,String> result){
        for(Map.Entry<String,String> tuple :  result.entrySet()){
            Hypervisor.Output(String.format("\"%s\" => \"%s\"",tuple.getKey(),tuple.getValue()));
        }
        return true;
    }
}
