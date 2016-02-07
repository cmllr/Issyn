package com.issyn;

import com.issyn.Data.DataBase;
import com.issyn.Data.Index;
import com.sun.org.apache.xpath.internal.operations.Bool;

import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;
import java.net.MalformedURLException;
import java.net.URL;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Main {

    public static void main(String[] args) throws MalformedURLException, InterruptedException {
        disableSSLCertificateChecking();
        Hypervisor h = new Hypervisor();
        h.Start();
        h.Run(h.ReadSeed(),null);
        h.WriteSeed();
        System.out.println(String.format("Created %s requests.",Downloader.DownloadsDone));
        System.out.println(String.format("%s entries not processed. Pushing back to seed.",Downloader.NotProcessed != null ? Downloader.NotProcessed.size() : 0));
    }
    private static void disableSSLCertificateChecking() {
        TrustManager[] trustAllCerts = new TrustManager[] { new X509TrustManager() {
            public X509Certificate[] getAcceptedIssuers() {
                return null;
            }

            @Override
            public void checkClientTrusted(X509Certificate[] arg0, String arg1) throws CertificateException {
                // Not implemented
            }

            @Override
            public void checkServerTrusted(X509Certificate[] arg0, String arg1) throws CertificateException {
                // Not implemented
            }
        } };

        try {
            SSLContext sc = SSLContext.getInstance("TLS");

            sc.init(null, trustAllCerts, new java.security.SecureRandom());

            HttpsURLConnection.setDefaultSSLSocketFactory(sc.getSocketFactory());
        } catch (KeyManagementException e) {
            e.printStackTrace();
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        }
    }
}
