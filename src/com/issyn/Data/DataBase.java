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
                conn = DriverManager.getConnection("jdbc:mysql://192.168.178.72/issyn?user=root&password=");
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
        java.text.SimpleDateFormat sdf =new java.text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        PreparedStatement statement = null;
        Boolean result = false;
        Boolean isinindex = this.IsInIndex(i.Page,i.Raw,i.Title);
        if (isinindex){
            this.UpdateLastIndex(i.Page);
            return false;
        }
        try {
            statement = this.GetConn().prepareStatement("Insert into site (page,cms,parent,indexed,lastindexed,title,hash,length) VALUES (?,?,?,?,?,?,?,?)");
            statement.setString(1,i.Page.toString());
            statement.setString(2,i.CMS);
            statement.setInt(3,0); //TODO
            statement.setString(4,sdf.format(i.Indexed));
            statement.setString(5,sdf.format(i.LastIndexed));
            statement.setString(6,i.Title);
            statement.setString(7,this.Hash(i.Raw));
            statement.setInt(8,i.Raw.length());
            result =  statement.executeUpdate() > 0;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return result;
    }
    public Boolean IsInIndex(URL site,String content,String title){
       PreparedStatement selection = null;
        Boolean result = false;
        try {
            selection = this.GetConn().prepareStatement("Select count(id) as amount from site where page = ? or hash = ? or (title = ? and length = ?)");
            selection.setString(1,site.toString());
            selection.setString(2,this.Hash(content));
            selection.setString(3,title);
            selection.setInt(4,content.length());
            ResultSet rs = selection.executeQuery();
            rs.next();
             result =  rs.getInt("amount") != 0;
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }
    private Boolean UpdateLastIndex(URL site){
        java.text.SimpleDateFormat sdf =new java.text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        Date now = new Date();
        String query = String.format("Update site set lastindexed = '%s'where page = '%s'",sdf.format(now),site.toString());
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
