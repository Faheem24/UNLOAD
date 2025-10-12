# UNLOAD (Unauthorized Login Attack Detector)
A self developed Proof-of-Concept (PoC) of basic SIEM functionality on windows. Detects basic abuse scenarios.

POC Components:
1. API Server
2. Database
3. Agent VM
4. Remote VM
5. UI

3 Visual Studio Projects:
1. SADAgentService:
Runs on Agent VM. Collects Logs and Sends to Server API. Service is build on Server and Installed on Agent VM.
2. UNLOADAPI:
API Runs on Server. Receives Logs from SADAgentService and writes onto Database. MVC Architecture.
3. UNLOAD Web UI:
Web User Interface. To See Logs and Alerts. Invokes 3 Detection Algorithms and Generates Alerts.

SQL Database:
UNLOADDB: Runs on Server. Connected to UNLOADAPI and UNLOAD Web UI.

For this project:
Server IP: 192.168.50.1
Agent VM IP: 192.168.50.5
Remote VM IP: Any desired IP can be set.

Detection Algorithms:
1. Password Guess:
Whenever there are multiple i.e. 5 failed login attempts of User "Sana" in less than 1 minute, Alert will be generated.
2. Remote Login:
If any user remotely logs into Agent VM with IP "192.168.50.5" from Remote machine with IP outside of allowed range i.e. 192.168.50.10 - 192.168.50.20. Alert will be generated.
3. Non Office Hours Login:
If User logs into any Agent VM outside of allowed time i.e. from 1:00 to 6:00. Alert will be generated.

Configurations can be changed through Config Page.
