package com.issyn;

import com.issyn.Data.Index;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by fury on 24.01.2016.
 */
public class Hypervisor {
    public static Index[] indizes = null;
    public static int MAXREQUESTS = 15;
    public void Start(){
        System.setProperty("http.agent","Mozilla/5.0 (compatible; Issyn2/0)");
        System.out.println(String.format("Starting Issyn (%s)...",System.getProperty("http.agent")));
    }
    public void Run(String[] seed,URL parent) throws MalformedURLException, InterruptedException {
        Thread.sleep(500);
        for (int i = 0; i < seed.length;i++){
            Crawler c = new Crawler(new URL(seed[i]));
            Index got = c.Crawl(parent);

            Boolean onIndex = false;
            if (indizes != null && got != null){
                for(Index ix : indizes){
                    if (ix.Page.toString().equals(got.Page.toString())){
                        //Site is already in index
                        System.out.println(String.format("Site \"%s\" (%s) (Parent: %s) is already on the index.",ix.Title,ix.Page, (parent != null) ? parent : ""));
                        if (ix.Hyperlinks.length == got.Hyperlinks.length){
                            //if the amount of hyperlinks (or other informations has not changed, the page is already on the index)
                            onIndex = true;
                            break;
                        }
                    }
                }
                if (!onIndex){
                    System.out.println(String.format("Site \"%s\" (%s) (Parent: %s) is new.",got.Title, got.Page,(parent != null) ? parent : ""));
                    Index[] newIndex = new Index[indizes.length+1];
                    for(int x = 0; x < indizes.length;x++){
                        newIndex[x] = indizes[x];
                    }
                    newIndex[indizes.length] = got;
                    indizes = newIndex;
                    System.out.println(String.format("Crawling children of %s",got.Page));
                    this.Run(got.Hyperlinks,got.Page);
                }
            }
            else if (got != null){
                indizes = new Index[1];
                indizes[0] = got;
                System.out.println(String.format("Crawling children of %s",got.Page));
                this.Run(got.Hyperlinks,got.Page);
            }
        }
    }
    public String[] ReadSeed(){
        BufferedReader br = null;
        List<String> seed = new ArrayList();
        try {

            String sCurrentLine;
            br = new BufferedReader(new FileReader(String.valueOf(Paths.get(System.getProperty("user.dir") , "seed.txt"))));
            while ((sCurrentLine = br.readLine()) != null) {
                seed.add(sCurrentLine);
            }

        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                if (br != null)br.close();
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }
        return seed.toArray(new String[]{});
    }
    public void WriteSeed(){

    }
}
