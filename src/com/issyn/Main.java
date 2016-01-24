package com.issyn;

import com.issyn.Data.Index;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Main {
    private static Index[] indizes;
    public static void main(String[] args) throws MalformedURLException {

	    Crawler c = new Crawler(new URL("http://0fury.de"));
        Index got = c.Crawl();
        indizes = new Index[0];
        System.out.println(String.format("Created %s requests.",Downloader.DownloadsDone));
        Boolean onIndex = false;
        for(Index ix : indizes){
            if (ix.Page.toString().equals(got.Page.toString())){
                //Site is already in index
                System.out.println(String.format("Site %s is already on the index.",ix.Page));
                if (ix.Hyperlinks.length == got.Hyperlinks.length){
                    //if the amount of hyperlinks (or other informations has not changed, the page is already on the index)
                    onIndex = true;
                    break;
                }
            }
        }
        if (!onIndex){
            System.out.println(String.format("Site %s is new.",got.Page));
        }
    }
}
