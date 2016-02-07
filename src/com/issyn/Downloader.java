package com.issyn;

import java.io.*;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Created by fury on 24.01.2016.
 */
class Downloader {
    public static int DownloadsDone = 0;
    public static List<URL> NotProcessed;
    public static Map<String,List<String>> Robots;
    public static List<String> GetDisallowed(String Robots){
        String[] blocks = Robots.split("User-agent:");
        List<String> disallowed = new ArrayList<>();
        for (int i = 0; i < blocks.length;i++){
            if (blocks[i] != ""){
                String[] lines = blocks[i].split("\n");
                Boolean affected = false;
                for(int x =0;x < lines.length;x++){
                    if (lines[x].contains(Hypervisor.USERAGENT) || lines[x].contains("*")){
                        //Line is for the crawler or universal for every crawler
                        affected = true;
                    }
                    if (lines[x].matches( ".*(D|d)isallow.*" ) && affected){
                        String relative = lines[x].replaceAll(".*(D|d)isallow:\\s{0,}","");
                        if (!disallowed.contains(relative) && !relative.equals("")){
                            disallowed.add(disallowed.size(),relative);
                        }
                    }
                }

            }
        }
        return disallowed;
    }
    public static String DownloadSite(URL target){
        if (Hypervisor.MAXREQUESTS != -1 && DownloadsDone >= Hypervisor.MAXREQUESTS){
            if ( Downloader.NotProcessed ==null){
                Downloader.NotProcessed = new ArrayList<>();
            }
            if (!Downloader.NotProcessed.contains(target)){
                NotProcessed.add(NotProcessed.size(),target);
            }
            return "";
        }
        //Check robots
        if (Hypervisor.RobotsFiles == null){
            Hypervisor.RobotsFiles = new HashMap();
        }
        String host = target.getHost();
        if (!Hypervisor.RobotsFiles.containsKey(host)){
            Hypervisor.RobotsFiles.put(host,null);
            try{
                //Download robots File
                String url = target.getProtocol()+"://"+host+"/robots.txt";
                String robots = Downloader.DownloadSite(new URL(url));
                List<String> disallowed = Downloader.GetDisallowed(robots);
                Hypervisor.RobotsFiles.remove(host);
                Hypervisor.RobotsFiles.put(host,disallowed);
            }
            catch(Exception ex){

            }
        }
        List<String> disallowedURI = Hypervisor.RobotsFiles.get(host);
        if (disallowedURI != null){

            for (String disallowed : disallowedURI){
                if (!target.getPath().equals("") && (target.getPath().startsWith(disallowed) || target.getPath() == disallowed)){
                    System.out.println(String.format("Site %s is not allowed. Denied by rule %s",target.toString(),disallowed));
                    return "";
                }
            }
        }
        String content = "";
        InputStream is = null;
        BufferedReader br;
        String line;

        try {
            is = target.openStream();  // throws an IOException
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
            Downloader.DownloadsDone++;
        }
        return content;
    }
}
