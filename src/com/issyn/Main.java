package com.issyn;

import com.issyn.Data.Index;

import java.net.MalformedURLException;
import java.net.URL;

public class Main {

    public static void main(String[] args) throws MalformedURLException {
	    Crawler c = new Crawler(new URL("http://0fury.de"));
        Index got = c.Crawl();
    }
}
