package com.chmr;


import com.chmr.Interfaces.IExtractor;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import java.util.Map;

/**
 * Created by fury on 01.10.16.
 */
public class Hypervisor {
    private URL target = null;
    public Hypervisor(URL target){
        this.target = target;
        System.setProperty("http.agent",Configuration.USERAGENT);
        Output(String.format("Run Issyn (%s) => %s",System.getProperty("http.agent"),target.toString()));
    }
    public static void Output(String output){
        Output(output,false);
    }
    public static void Output(String output,Boolean IsCritical){
        if (Configuration.IsVerbose){
            System.out.println((IsCritical) ? "[E]: ": "[I]: " + output);
        }
    }
    public String Download(URL target){
        String host = target.getHost();
        String content = "";
        InputStream is = null;
        BufferedReader br;
        String line;
        try {
            URLConnection foo = target.openConnection();
            HttpURLConnection conn = (HttpURLConnection)foo;
            conn.setInstanceFollowRedirects(true);
            HttpURLConnection.setFollowRedirects(true);
            conn.addRequestProperty("User-Agent", Configuration.USERAGENT);
            is = foo.getInputStream();
            br = new BufferedReader(new InputStreamReader(is));
            while ((line = br.readLine()) != null) {
                content += line + "\n";
            }
        } catch (MalformedURLException mue) {
            mue.printStackTrace();
        } catch (FileNotFoundException fe){
            //Do nothing. Its 404.
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            try {
                if (is != null) is.close();
            } catch (IOException ioe) {
                // nothing to see here
            }
        }
        return content;
    }
    public Boolean Run() throws ClassNotFoundException, IllegalAccessException, InstantiationException {

        String content = this.Download(this.target);
        Output(String.format("Grabbed %s => %s chars",this.target,content.length()));
        String[] extractors = new String[]{"MetaExtractor"};
        for(String extractor: extractors){
            Output(String.format("Running extractor \"%s\"",extractor));
            Class<?> clazz = Class.forName("com.chmr.Extractors."+extractor);
            IExtractor ext = (IExtractor) clazz.newInstance();
            Map<String,String> got = ext.Extract(content);
            Output(String.format("Extractor \"%s\" found %s key value pairs",extractor,got.size()));
            Boolean save = ext.Store(got);
            Output(String.format("Extractor \"%s\" saved results: %s",extractor,(save) ? "yes" : "no"),!save);
            int x=0;
        }

        return false;
    }
}


