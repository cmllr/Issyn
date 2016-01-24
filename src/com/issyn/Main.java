package com.issyn;

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
        //TODO: Load indezes. Each site has is own collection
        h.Run(h.ReadSeed());
    }
}
