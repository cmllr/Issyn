package com.chmr;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

public class Main {
    public static List<String> allLinks = new ArrayList<String>();
    public static void main(String[] args) throws MalformedURLException, IllegalAccessException, InstantiationException, ClassNotFoundException {
            //TODO: Run from seed
            URL target = new URL("http://web.fritz.box/GitHub/");
	        Hypervisor hyp = new Hypervisor(target);
            hyp.Run();
    }
}
