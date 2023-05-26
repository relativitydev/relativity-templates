# relativity-templates
The Relativity Templates are compatible with Visual Studio 2017 and 2019 and consist of the following:
- Agent template
- Custom page templates
    - Custom page MVC 5 template
    - Custom page Web Forms template
- Event Handler templates
    - Console Event handler template
    - List Page Interaction Event handler template
    - Page Interaction Event handler template
    - Post-Install Event handler template
    - Post-Save Event handler template
    - Pre-Cascade Delete Event handler template
    - Pre-Delete Event handler template
    - Pre-Install Event handler template
    - Pre-Load Event handler template
    - Pre-Mass Delete Event handler template
    - Pre-Save Event handler template
    - Pre-Uninstall Event handler template
- Manager-Worker Agent template
- Kepler (for Relativity Versions 10.3.191.8 and higher)
    - Kepler Project Template (creates Interfaces and Services projects in one go, has a UI Wizard)
    - Kepler Exception Item Template
    - Kepler Exception.FaultSafe Item Template
    - Kepler IModule Item Template
    - Kepler IService Item Template
    - Kepler Module Item Template
    - Kepler Serice Item Template

## Maintainers

This repository is maintained by the Eventing & API Enablement Team. Feel free to reach out to our team for any questions or requests.

---

### Installation
To install all of the templates simply double click the .vsix file and click the “Install” button. If Visual Studio is already open you will have to restart it for the templates to populate.

## Project Templates

To use these click File -> New Project
All the Relativity Project Templates will be found under:
- 	Visual C#/Relativity/Agent/
- 	Visual C#/Relativity/CustomPages/
- 	Visual C#/Relativity/EventHandlers/
- 	Visual C#/Relativity/ManagerWorker/
- 	Visual C#/Relativity/Kepler/

_* If there are any errors make sure you have all the references setup correctly._

## Item Templates

To use these right click on your desired solution and then click “add new item”
All the Relativity Item Templates will be found under:
- 	Visual C#/Relativity/Agent/
- 	Visual C#/Relativity/CustomPages/
- 	Visual C#/Relativity/EventHandlers/
- 	Visual C#/Relativity/Kepler/

_* Note that if the project you add the item to does not have the correct references added, Visual Studio will tell you that you are missing assembly references._

## Manager-Worker Agent template
The Manager-Worker Agent project template is an architectural pattern where a long-running Agent task can be split into single or multiple manager/worker agents and execute the task in parallel. This template also consists recommended best practices for coding and features like resource-pool aware agents, configure agents to run in off hours, a custom page to view manager/worker agents progress, unit tests projects etc.

The following projects are created:

 - **Agents**

	This project includes a manager agent and a worker agent.  Single or Multiple Manager/Worker agents can be installed in the environment.  The only thing you should need to change is the ProcessRecordsAsync method of the Job class.  This is where all of the agent work should happen.

 - **Agents.NUnit**

	Add any unit tests for the agent project to this project.

 - **CustomPages**

	This project includes custom pages to manage both the worker and the manager queue.  Once you create an application which includes the custom pages, you can create a tab to view each queue using the following URLs:

 */Relativity/CustomPages/<AppGuid>/ManagerAgent/Index/?StandardsCompliance=true&%AppID%*
 */Relativity/CustomPages/<AppGuid>/WorkerAgent/Index/?StandardsCompliance=true&%AppID%*

 - **CustomPages.NUnit**

	Add any unit tests for the custom pages project to this project.

 - **EventHandlers**

	This project includes a post-install event handler to create the underlying queue tables and error log table.  It also includes a console event handler which can be used to manage jobs in the manager queue using a custom object if you do not wish to use the custom pages to manage the jobs.

 - **EventHandlers.NUnit**

	Add any unit tests for the event handlers project to this project.

 - **Helpers**

	This project includes the shared code between the projects in the solution.  The only thing you should need to change are these three constants in the Constant class:

*ApplicationGuid* – the GUID of the application
*ManagerQueueTab* – the GUID of the tab which displays the manager queue custom page
*WorkerQueueTab* – the GUID of the tab which displays the worker queue custom page

 - **Helpers.NUnit**

	Add any unit tests for the helpers project to this project.
