Floccus
=======

A webcrawling bundle. It should help you to create a small search engine for example for your team to find content on _your_ websites. It's not meant for replacing the big search engines in the web. But when you have your local area network serving a view websites, the bundle could be a help

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
