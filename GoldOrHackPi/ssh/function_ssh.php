<?php

function getTtsFromCsvString($csv)
{
	
	
	$csv = str_replace("\r","", $csv);
	
    $csv_lines = explode("\n", $csv);
	
	// print_r($csv_lines);
	
    $tts_ssh = array();

    foreach ($csv_lines as $line)
    {
		
		$line = trim($line);
		
		$isVide = empty($line);
		
        if(!($isVide))
        {
            $tts_ssh[] = str_getcsv($line,";");
        }
		
    }

    return $tts_ssh;
}

function ssh_exec($host, $username, $password, $cmd)
{

///Tentative de connection

    require_once('Net/SSH2.php');

    $ssh = new Net_SSH2($host);

    if (!$ssh->login($username, $password))
    {
        exit('Login Failed');
    }

    $output = $ssh->exec($cmd);

    return $output;
}



