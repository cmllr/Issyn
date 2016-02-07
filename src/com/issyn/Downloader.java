package com.issyn;

import java.io.*;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by fury on 24.01.2016.
 */
class Downloader {
    public static int DownloadsDone = 0;
    public static List<URL> NotProcessed;
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
