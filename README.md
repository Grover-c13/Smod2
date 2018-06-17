# ServerMod2
ServerMod2 is a server side plugin system with a bunch of additional configuration options, bug fixes, security patches and some optimisations built in.

The latest release can be found here: [Release link](https://github.com/Grover-c13/Smod2/releases/latest)

## Discord
You can join our Discord here: https://discord.gg/8nvmMTr

## ServerMod Installation:
To install:
1. Navigate to your SCP Secret Lab folder.
2. Go into SCPSL_Data/Managed/
3. Make a backup of Assembly-CSharp.dll
4. Replace Assembly-CSharp.dll with the one in the releases tab and copy Smod2.dll from the releases tab into the same folder.

## Server Name Variables
Currently supported variables (place in your servers name):
- $player_count (current number of connected players) EG: "$player_count playing!"
- $port (the port of the current server) EG: "Welcome to SCPServer.com:$port"
- $ip (the ip of the server) EG: "Welcome to SCPServer.com [$ip:$port]"
- $full_player_count (will display player count as $player_count/$max_player_count or FULL if there are $max_player_count players) EG: "Server.com $full_player_count"
- $number (will display the number of the instance, assuming youre using default ports, this works by subtracting 7776 from the port (so $number will = 1 for the first server, #2 for the second)
- $lobby_id (debugging to print the lobby_id)
- $version (version of the game)
- $sm_version (version of ServerMod)
- $max_players (max amount of players in the config)
- $scp_alive - number of alive SCPS.
- $scp_start - number of SCPs at start of the round.
- $scp_counter - prints $scp_alive/$scp_start
- $scp_dead - number of dead scps.
- $scp_zombies - current number of zombies.
- $classd_escape - how many class ds have escaped.
- $classd_start - the amount of starting class ds.
- $classd_counter - $classd_escape/$classd_counter.
- $scientists_escape - The number of scientists to escape so far.
- $scientists_start - the amount of starting scientists
- $scientists_counter - $scientists_escape/$scientist_start.
- $scp_kills - number of people killed by scps.
- $warhead_detonated - prints ☢ WARHEAD DETONATED ☢ if its gone off.

Example:
![player count](https://user-images.githubusercontent.com/1520101/36029888-04689b5c-0de0-11e8-81cd-b1d458caf7e9.png)

## Config Additions
Type Info:
- Boolean: True or False value
- Integer: A number without decimals
- Float: A number with decimals (Formatting like "1.0" and "1,0" both work and are the same value)
- List: A list with items separated by ",", for example: `list: 1,2,3,4,5`
- Dictionary: A dictionary with items separated by ":", and each entry separated by ",", for example: `dictionary: 1:2,2:3,3:4`
- Seconds: Time in seconds, usually a value of -1 disables the feature
- Minutes: Time in minutes, usually a value of -1 disables the feature
- R: If the config option has an R before it, it means that you can use a random value in it. A random value is defined by having "{}", items listed like "weight%value" where if you don't put a weight it defaults to a weight of 1, separated by "|", for example: `rlist: {1%1|2%7|6},3,6,{15%3|2|45%2}`

Crossed out config options are removed, unless otherwise specified in the description

Bolded config options are ones that are in the vanilla game

### General
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
allow_incompatible | Boolean | False | Allow the server to run an incompatible version of ServerMod
auto_round_restart_time | Seconds | 10 | The time before the next round starts when a round ends
server_frame_rate | Integer | 60 | The framerate that a server runs at
show_on_serverlist | Boolean | True | If your server is verified, this shows it on the server list
sm_debug | Boolean | False | Print more verbose debug messages for debugging
sm_server_name | String | **Dynamic** | server name in a separate option, defaults to the value of server_name (You'd use this if you don't want variables showing up in your server name when ServerMod isn't working)
sm_tracking | Boolean | True | Appends the ServerMod version to your server name, this is for tracking how many servers are running ServerMod
~~master_server_to_contact~~ | String | https://hubertmoszka.pl/authenticator.php | The master server to push data to, this is used for private server lists **(DEPRICATED, USE "secondary_servers_to_contact")**
secondary_servers_to_contact | List | **Empty** | The master servers to push data to, this is used for private server lists
start_round_timer | Seconds | 20 | The amount of time before the round auto-starts (when queueing for a round)

### Administration / Gameplay
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
dedicated_slots | Integer | **Number of IPs in** `Reserved Slots.txt` | The number of slots above the maximum to reserve for certain players **(REQUIRES RESTART)**
dedicated_slot_location | String | \[appdata\]/SCP Secret Laboratory | The directory that the Reserved Slots file should be
dedicated_slot_file_name | String | Reserved Slots.txt | The file name to use for the Reserved Slots file
~~dedicated_slot_ips~~ | List | **Empty** | A list of the IPs of players to allow into the reserved slots **(Depricated, use "Reserved Slots.txt" instead, scroll down for the usage)**
reserved_slots_simulate_full | Boolean | False | For debugging, this simulates the server being full, so only players with reserved slots can **(Do not enable this if you don't know what you're doing)**
~~disable_badges~~ | Boolean | False | If true, admins will not have the admin badge on your server. **(DEPRICATED, use "hidden" as badge color in remote admin config instead)**
**disable_decontamination** | Boolean | False | Enables / Disables Light Containment Zone decontamination
enable_ra_server_commands | Boolean | True | Enables / Disables running console commands through text based Remote Admin
server_command_whitelist | List | **Empty** | A list of SteamID64s for the users allowed to run console commands through text based Remote Admin (Whitelist is used by default even if you don't specify it)
bypass_server_command_whitelist | Boolean | False | Allows anybody with access to text based Remote Admin to run console commands
filler_team_id | Integer | 4 | If the team spawn queue is shorter than the max player count, this team number will be used for the rest of the players when they spawn
item_cleanup | Seconds | -1 | Cleans up items after the specified amount of time
nickname_filter | List | **Empty** | Automatically kicks anyone who's nickname contains anything in this list
remove_item_loot | RList | **Empty** | Removes all instances of the specified item ID from all lockers
replace_item_loot | RDictionary | **Empty** | Replaces all instances of the specified item ID from all lockers with the second specified item ID
add_item_loot | RList | **Empty** | Adds the specified item ID to all lockers' loot
**scp_grenade_multiplier** | Float | 1.0 | The multiplier for the amount of damage grenades do to SCPs
**human_grenade_multiplier** | Float | 0.7 | The multiplier for the amount of damage grenades do to humans

#### Reserved Slots
How to use the new Reserved Slots:
In the new "Reserved Slots.txt" file in the SCP AppData location, you put one SteamID or IP per line, and you can end each line with a comment using "//". If you want to use both an IP and SteamID, you put the IP, a semicolon (";"), then the SteamID

Everything following and including the "//" is optional

Example Usage:
```
1.1.1.1 // IP
11111111111111111 // SteamID64 (Automatically fetches IP)
127.0.0.1;22222222222222222 // IP and SteamID64 (Automatically updates IP)
127.0.0.1;22222222222222222
127.0.0.1;11111111111111111
```

Known bugs:

Server might crash when first creating "Reserved Slots.txt", just restart the server if this happens and it shouldn't happen again.

### Player Management
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
afk_kick | Seconds | -1 | Kicks players who haven't moved in a specified amount of time
escapee_restrained_check | Boolean | False | If true, escapees are set to the opposite team if they are cuffed (disarmed), for example, if a Class-D escaped while cuffed, they would become NTF
~~last_movement_timeout~~ | Seconds | 30 | After this amount of time without a player sending any movement, they will be kicked (still sends movement if they're standing still, so this isn't anti-afk)
~~rejected_movement_limit~~ | Integer | -1 | The amount of movements detected by the anti-cheat as invalid before a player is kicked, the detection count increases per invalid movement and decreases per valid movement
~~sm_onplayerjoin_tries_timeout~~ | Integer | 50 | The amount of tries before the OnPlayerJoin event gives up on executing for a player (to prevent it constantly running if a player disconnects before it's run)

### Warhead Options
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
auto_warhead_start | Seconds | -1 | Automatically activated the nuke after the specified amount of time has elapsed (-1 disables this feature)
auto_warhead_start_lock | Boolean | False | Automatically prevents the warhead detonation from being cancelled when it's automatically started

### SCP-914 Options
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
SCP914_teleport_players | Boolean | True | Moves players in SCP-914's input area to the output area
SCP914_keep_health | Boolean | True | Keep the same health when a player moves from SCP-914's input area to the output area and the class is changed
SCP914_<rough/coarse/1_to_1/fine/very_fine>_change_class | RDictionary | **Empty** | Changes a player's class from the first specified class to the second specified class when they're teleported to SCP-914's output area
SCP914_in_<rough/coarse/1_to_1/fine/very_fine>_damage | RDictionary | **Empty** | Damages a player by the second specified value when the class matches the first specified value before their class is changed
SCP914_out_<rough/coarse/1_to_1/fine/very_fine>_damage | RDictionary | **Empty** | Damages a player by the second specified value when the class matches the first specified value after their class is changed

### Pocket Dimension Options
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
pd_exit_count | Integer | 2 | The amount of exits to the Pocket Dimension
pd_random_exit | Boolean | False | If true, players will be teleported to a random place after escaping from Pocket Dimension
pd_random_exit_ignore_rids | List | **Empty** | The list of RoomIDs that won't be used when pd_random_exit is enabled
pd_refresh_exit | Boolean | False | Randomly refresh the exit of Pocket Dimension after it's used

### Class Based
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
no_scp079_first | Boolean | True | Computer (SCP-079) will never be the first scp in a game
173_door_starting_cooldown | Seconds | 25 | The time before SCP-173's door can be opened
**SCP106_cleanup** | Boolean | False | Stops items and ragdolls from spawning in the pocket dimension
maximum_MTF_respawn_amount | Integer | 15 | The maximum amount of MTF that can be respawned in a single respawn wave
SCP049_HP | Integer | 1200 | Sets the starting HP for SCP-049
SCP049-2_HP | Integer | 400 | Sets the starting HP for SCP-049-2
SCP079_HP | Integer | 100 | Sets the starting HP for SCP-079
SCP096_HP | Integer | 2000 | Sets the starting HP for SCP-096
SCP106_HP | Integer | 700 | Sets the starting HP for SCP-106
SCP173_HP | Integer | 3200 | Sets the starting HP for SCP-173
~~SCP457_HP~~ | Integer | 700 | Sets the starting HP for SCP-457
CLASSD_HP | Integer | 100 | Sets the starting HP for Class Ds
SCIENTIST_HP | Integer | 100 | Sets the starting HP for Scientists
CI_HP | Integer | 100 | Sets the starting HP for Chaos Insurgency
NTFG_HP | Integer | 100 | Sets the starting HP for NTF Guards
NTFSCIENTIST_HP | Integer | 120 | Sets the starting HP for NTF Scientists
NTFL_HP | Integer | 120 | Sets the starting HP for NTF Lieutenants
NTFC_HP | Integer | 150 | Sets the starting HP for NTF Commanders
FACILITYGUARD_HP | Integer | 100 | Sets the starting HP for Facility Guards
force_disable_enable | Boolean | False | Overrides game's default class ban value with chosen values **(USE OF THIS IS NOT RECOMMENDED)**
SCP049_DISABLE | Boolean | False | Disables SCP-049
SCP079_DISABLE | Boolean | True | Disables SCP-079
SCP096_DISABLE | Boolean | False | Disables SCP-096
SCP106_DISABLE | Boolean | False | Disables SCP-106
SCP173_DISABLE | Boolean | False | Disables SCP-173
~~SCP457_DISABLE~~ | Boolean | True | Disables SCP-457
SCP049_AMOUNT | Integer | 1 | Max amount of SCP-049 that can be spawned in randomly
SCP079_AMOUNT | Integer | 1 | Max amount of SCP-079 that can be spawned in randomly
SCP096_AMOUNT | Integer | 1 | Max amount of SCP-096 that can be spawned in randomly
SCP106_AMOUNT | Integer | 1 | Max amount of SCP-106 that can be spawned in randomly
SCP173_AMOUNT | Integer | 1 | Max amount of SCP-173 that can be spawned in randomly
~~SCP457_AMOUNT~~ | Integer | 1 | Max amount of SCP-457 that can be spawned in randomly

### Smart Class Picker (All in Vanilla Game)
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
**smart_class_picker** | Boolean | False | Enables/Disables Smart Class Picker
**smart_cp_starting_weight** | Integer | 6 | The weight a class starts out with
**smart_cp_weight_min** | Integer | 1 | The minimum weight a class can have
**smart_cp_weight_max** | Integer | 11 | The maximum weight a class can have
**smart_cp_class_<Class #>_weight_decrease** | Integer | **Dynamic** | The amount a weight goes down when a player plays the specified class, the default value is dynamic based on which team and class the player is
**smart_cp_class_<Class #>_weight_increase** | Integer | **Dynamic** | The amount a weight goes up when the player isn't the specified class, the default value is dynamic based on which team and class the player is
**smart_cp_team_<Team #>_weight_decrease** | Integer | **Dynamic** | The amount the weight for each class on a team goes down when a player plays on the specified team, the default value is dynamic based on which team and class the player is
**smart_cp_team_<Team #>_weight_increase** | Integer | **Dynamic** | The amount the weight for each class on a team goes up when the player isn't on the specified team, the default value is dynamic based on which team and class the player is


#### Default Functionality
- Every class gets +1 weight except for the class the player is chosen to be or the chosen class is NTF or SCP
- If the player is chosen to be NTF, the chosen class gets -4 weight and every other NTF class gets -2 weight
- If the player is chosen to be SCP, the chosen class gets -3 weight and every other SCP class gets -2 weight
- If the player is chosen to be Class D, Class D gets -3 weight
- If the player is chosen to be any other class, the chosen class gets -2 weight

##

Place any suggestions/problems in issues!

Thanks & Enjoy.

