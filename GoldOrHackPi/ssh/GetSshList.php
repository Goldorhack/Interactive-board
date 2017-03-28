<?php

require_once('function_ssh.php');

/// File : GetSshList.php <br/>
/// Author : Felix ROBICHON <br/>
/// Create Date : 2017-02-06 <br/>
/// Last Update : 2017-02-06 <br/>

/// phase 01 : recuperer les données de connection
require_once('../config/ssh_serv_list.php');
$tts_ssh = getTtsFromCsvString($ssh_csv_info);

/// phase 02 : afficher le nom des hotes

for ($i = 0; $i < count($tts_ssh); $i++)
{
    echo $tts_ssh[$i][0];
    echo "<br />\n";
}
