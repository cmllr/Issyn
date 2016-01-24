package com.issyn;

import com.issyn.Data.Index;

import java.net.MalformedURLException;
import java.net.URL;

/**
 * Created by fury on 24.01.2016.
 */
public class Hypervisor {
    private static Index[] indizes = null;
    public static int MAXREQUESTS = 15;
    public void Start(){
        System.setProperty("http.agent","Mozilla/5.0 (compatible; Issyn2/0)");
        System.out.println(String.format("Starting Issyn (%s)...",System.getProperty("http.agent")));
    }
    public void Run(String[] seed) throws MalformedURLException, InterruptedException {
        Thread.sleep(500);
        for (int i = 0; i < seed.length;i++){
            Crawler c = new Crawler(new URL(seed[i]));
            Index got = c.Crawl();
            System.out.println(String.format("Created %s requests.",Downloader.DownloadsDone));
            Boolean onIndex = false;
            if (indizes != null && got != null){
                for(Index ix : indizes){
                    if (ix.Page.toString().equals(got.Page.toString())){
                        //Site is already in index
                        System.out.println(String.format("Site \"%s\" (%s)  is already on the index.",ix.Title,ix.Page));
                        if (ix.Hyperlinks.length == got.Hyperlinks.length){
                            //if the amount of hyperlinks (or other informations has not changed, the page is already on the index)
                            onIndex = true;
                            break;
                        }
                    }
                }
                if (!onIndex){
                    System.out.println(String.format("Site \"%s\" (%s) is new.",got.Title, got.Page));
                    Index[] newIndex = new Index[indizes.length+1];
                    for(int x = 0; x < indizes.length;x++){
                        newIndex[x] = indizes[x];
                    }
                    newIndex[indizes.length] = got;
                    indizes = newIndex;

                    this.Run(got.Hyperlinks);
                }
            }
            else if (got != null){
                indizes = new Index[1];
                indizes[0] = got;

                this.Run(got.Hyperlinks);
            }
        }
        System.out.println(String.format("Index has now %s entries.",indizes.length));
        System.out.println(String.format("%s entries not processed. Pushing back to seed.",Downloader.NotProcessed));
    }
    public String[] ReadSeed(){
        return  new String[]{""};
    }
    public void WriteSeed(){

    }
}
