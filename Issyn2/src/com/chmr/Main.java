package com.chmr;

import java.net.MalformedURLException;
import java.net.URL;

public class Main {

    public static void main(String[] args) throws MalformedURLException, IllegalAccessException, InstantiationException, ClassNotFoundException {
            //TODO: Run from seed
            URL target = new URL("http://web.fritz.box/Kamera/");
	        Hypervisor hyp = new Hypervisor(target);
            hyp.Run();
    }
}
