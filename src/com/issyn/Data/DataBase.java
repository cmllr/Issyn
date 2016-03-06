package com.issyn.Data;

import java.net.URL;
import java.security.MessageDigest;
import java.sql.*;
import java.util.Date;

/**
 * Created by fury on 07.02.2016.
 */
public class DataBase {
    static Connection conn = null;
    private static DataBase instance = null;
    public static DataBase GetInstance(){
        try{
            Class.forName("com.mysql.jdbc.Driver");
            if (DataBase.conn == null){
                conn = DriverManager.getConnection("jdbc:mysql://192.168.178.68/issyn?user=issyn&password=dBN2bWnheAR8QFfU");
                DataBase.instance = new DataBase();
            }
        }
        catch(Exception e){
            e.printStackTrace();
        }
        return instance;
    }
    private Connection GetConn(){
        return DataBase.conn;
    }
    public static String Hash(String input) {
        try {
            java.security.MessageDigest md = java.security.MessageDigest.getInstance("MD5");
            byte[] array = md.digest(input.getBytes());
            StringBuffer sb = new StringBuffer();
            for (int i = 0; i < array.length; ++i) {
                sb.append(Integer.toHexString((array[i] & 0xFF) | 0x100).substring(1,3));
            }
            return sb.toString();
        } catch (java.security.NoSuchAlgorithmException e) {
        }
        return null;
    }
    public Boolean InsertIndex(Index i){
        String host = i.Page.getHost();
        String hashToSearch = this.Hash(host);
        java.text.SimpleDateFormat sdf =new java.text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        PreparedStatement statement = null;
        Boolean result = false;
        Boolean isinindex = this.IsInIndex(i.Page,i.Raw,i.Title);
        if (isinindex){
            this.UpdateLastIndex(i.Page);
            return false;
        }
        try {
            statement = this.GetConn().prepareStatement("Insert into "+hashToSearch+" (url,cms,indexed,lastindexed,content,hash,title) VALUES (?,?,?,?,?,?,?)");
            statement.setString(1,i.Page.toString());
            statement.setString(2,i.CMS);
            statement.setString(3,sdf.format(i.Indexed));
            statement.setString(4,sdf.format(i.LastIndexed));
            statement.setString(5,i.Raw);
            statement.setString(6,this.Hash(i.Raw));
            statement.setString(7,i.Title);
            result =  statement.executeUpdate() > 0;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return result;
    }
    public Boolean CreateIndexLeaflet(URL site){
        String host = site.getHost();
        String hashToSearch = this.Hash(host);
        String query = String.format("CREATE TABLE %s LIKE dummy;",hashToSearch);
        Statement selection = null;
        Boolean result = false;
        try {
            selection = this.GetConn().createStatement();
            selection.executeUpdate(query);
            result =  this.IsTableExisting(site);
        } catch (SQLException e) {
            e.printStackTrace();
        }
        if (result){
            System.out.println("Created leaflet for " +host);
        }
        return result;
    }
    public Boolean IsTableExisting(URL site){
        PreparedStatement selection = null;
        Boolean result = false;
        String host = site.getHost();
        String hashToSearch = this.Hash(host);
        try {
            selection = this.GetConn().prepareStatement("SELECT count(table_name) as amount \n" +
                    "FROM information_schema.tables\n" +
                    "WHERE table_schema = 'issyn'\n" +
                    "AND table_name = ?;");
            selection.setString(1,hashToSearch);
            ResultSet rs = selection.executeQuery();
            rs.next();
            result =  rs.getInt("amount") != 0;
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }
    public Boolean IsInIndex(URL site,String content,String title){
        String host = site.getHost();
        String hashToSearch = this.Hash(host);
        Boolean existing = this.IsTableExisting(site);
        //Create table leaflet if needed
        if (!existing){
            this.CreateIndexLeaflet(site);
            return false;
        }
       PreparedStatement selection = null;
        Boolean result = false;
        try {
            selection = this.GetConn().prepareStatement("Select count(id) as amount from "+hashToSearch+" where url = ? or hash = ? or (title = ?)");
            selection.setString(1,site.toString());
            selection.setString(2,this.Hash(content));
            selection.setString(3,title);
            ResultSet rs = selection.executeQuery();
            rs.next();
            result =  rs.getInt("amount") != 0;
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }
    private Boolean UpdateLastIndex(URL site){
        String host = site.getHost();
        String hashToSearch = this.Hash(host);
        java.text.SimpleDateFormat sdf =new java.text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        Date now = new Date();
        String query = String.format("Update " + hashToSearch+" set lastindexed = '%s'where url = '%s'",sdf.format(now),site.toString());
        Statement selection = null;
        Boolean result = false;
        try {
            selection = this.GetConn().createStatement();
            result =  selection.executeUpdate(query) == 1;
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }
}
