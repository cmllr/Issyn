Issyn
=======

Issyn is a simple webcrawler without a strict goal. It harvests the metadata from the sites (CMS, Tags etc.) and stores it into a database. Currently there is no frontend.

The parts
=========

The crawler Issyn and the web frontend (currently no cool internal name given).

The plan
=====

The plan is to create a distributed web crawler. 

* There is one or more master node which _only_ stores the database.
* There is one or more webmaster node(s) which servers the website frontend.
* There are **mutliple** crawler slaves which are indexing websites. 

The state
========

Bleeding edge to the max!
