package com.issyn;

import com.issyn.Data.DataBase;
import com.issyn.Data.Index;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Main {

    public static void main(String[] args) throws MalformedURLException, InterruptedException {
        Hypervisor h = new Hypervisor();
        h.Start();
        h.Run(h.ReadSeed(),null);
        h.WriteSeed();
        System.out.println(String.format("Created %s requests.",Downloader.DownloadsDone));
        System.out.println(String.format("%s entries not processed. Pushing back to seed.",Downloader.NotProcessed != null ? Downloader.NotProcessed.size() : 0));
    }
}
