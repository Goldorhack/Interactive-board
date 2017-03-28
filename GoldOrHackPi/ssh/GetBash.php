<?php

require_once('function_ssh.php');

/// phase 01 : recuperer le host selectioné.

$host = "";
$username = "";
$password = "";

$host_get = "";

if (isset($_GET["host"]))
{
    $host_get = $_GET["host"];
}

/// phase 02 : recuperer les données de connection

require_once('../config/ssh_serv_list.php');

$tts_ssh = getTtsFromCsvString($ssh_csv_info);

for($i = 0; $i < count($tts_ssh); $i++)
{
    if($host_get == $tts_ssh[$i][0] )
    {
        $host = $tts_ssh[$i][0];
        $username = $tts_ssh[$i][1];
        $password = $tts_ssh[$i][2];
        break;
    }
}

if(empty($host))
{
    $host = $tts_ssh[0][0];
    $username = $tts_ssh[0][1];
    $password = $tts_ssh[0][2];
}


//echo " $host ; $username ";
//echo "<br />\n";
//echo "<br />\n";

/// phase 03 : recuperer les commandes
$cmd_get_bash = 'cat ~/.GoldOrHack/bash.csv.txt';
$csv_bash = ssh_exec($host, $username, $password, $cmd_get_bash);

/// phase 04 : afficher le csv des bash
echo $csv_bash;
