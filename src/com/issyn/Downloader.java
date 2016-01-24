package com.issyn;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;

/**
 * Created by fury on 24.01.2016.
 */
class Downloader {
    public static int DownloadsDone = 0;
    public static int NotProcessed = 0;
    public static String DownloadSite(URL target){
        if (Hypervisor.MAXREQUESTS != -1 && DownloadsDone >= Hypervisor.MAXREQUESTS){
            NotProcessed++;
            return "";
        }
        String content = "";
        URL url;
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
        } catch (IOException ioe) {
            ioe.printStackTrace();
        } finally {
            try {
                if (is != null) is.close();
            } catch (IOException ioe) {
                // nothing to see here
            }
        }
        Downloader.DownloadsDone++;
        return content;
    }
}
