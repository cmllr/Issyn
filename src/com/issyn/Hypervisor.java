package com.issyn;

import com.issyn.Data.DataBase;
import com.issyn.Data.Index;

import java.io.*;
import java.net.MalformedURLException;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Created by fury on 24.01.2016.
 */
public class Hypervisor {
    public static Index[] indizes = null;
    public static int MAXREQUESTS = 20;
    public static final String USERAGENT = "Mozilla/5.0 (compatible; Issyn2/0)";
    public static Map<String,List<String>> RobotsFiles;
    public void Start(){
        System.setProperty("http.agent",Hypervisor.USERAGENT);
        System.out.println(String.format("Starting Issyn (%s)...",System.getProperty("http.agent")));
    }
    public void Run(String[] seed,URL parent) throws MalformedURLException, InterruptedException {
        Thread.sleep(500);
        for (int i = 0; i < seed.length;i++){
            URL href = new URL(seed[i]);
            Crawler c = new Crawler(href);
            Index got = c.Crawl(parent);

            Boolean onIndex = got != null ? DataBase.GetInstance().IsInIndex(href,DataBase.Hash(got.Raw),got.Title) : false;
            if (indizes != null && got != null){
                if (!onIndex){

                    System.out.println(String.format("Site \"%s\" (%s) (Parent: %s) is new.",got.Title, got.Page,(parent != null) ? parent : ""));
                    Index[] newIndex = new Index[indizes.length+1];
                    for(int x = 0; x < indizes.length;x++){
                        newIndex[x] = indizes[x];
                    }
                    newIndex[indizes.length] = got;
                    indizes = newIndex;
                    System.out.println(String.format("Crawling children of %s",got.Page));
                    if (DataBase.GetInstance().InsertIndex(got)){
                        this.Run(got.Hyperlinks,got.Page);
                    }
                    else{
                        System.out.println(String.format("%s is a recursive trap or already indexed.",got.Page));
                    }


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
        try{

            List<URL> notProcessed = Downloader.NotProcessed;
            if (notProcessed == null){
                System.out.println(String.format("No seed remaining"));
            }
            else{
                File seed = new File(Paths.get(System.getProperty("user.dir") , "seed.txt").toString());
                seed.delete();
                FileWriter writer = new FileWriter(seed.getAbsolutePath(),true);
                for(URL href: notProcessed) {
                    writer.write(href.toString() + "\n");
                }
                writer.close();
            }
        }catch(Exception e){
            e.printStackTrace();
        }
    }
}
