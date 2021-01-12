# Mars Rover - Image Downloader 

The Mars Rover - Image Downloader is a simple Web API that downloads images taken by the Mars Rover. It leverages on the Mars Rover API, and a text file containing a list of dates.

## Local Development
### Prerequisites 
Mars Rover - Image Downloader requires the following:
- dotnet core 3.1
- API client (such as Postman)

### Building for source
```bash
$ dotnet build
```

#### Running the source
```bash
$ dotnet run
```

#### Testing the app
The app can be tested using any API client, such as Postman. For convenience, a Postman collection and environment is available in \Postman. 

A text file (*.txt) with line separated dates is required to download Mars Rover images, such that:

02/27/17
June 2, 2018
Jul-13-2016
April 31, 2018

An image will be selected per valid date in the text file, and downloaded to \Images.

#### Logs
Error logs are available at C:\MarsRover\Logs. Logs are automatically rolled up per day.


#### Postman setup
##### Setting up Postman collection and environment
- In Postman, click File > Import (Ctrl + O)
- In the Import dialog, click the File tab
- Click Upload Files
- In the Open dialog, browse to the \Postman folder and select the collection (*.postman_collection.json) or environment (*.postman_environment.json) file.
- Click Open
- Click Import

##### Selecting the environment
- In Postman, click the No environment drop-down list at the top-right corner of the app.
- Select the MarsRover environment from the list

##### Updating environment variables
- In Postman, select the MarsRover environment
- Click the eye icon to the right of the environment drop-down list
- In the pop-up, click Edit
- In the Manage Environments dialog, update the current value of the relevant variable
- Click Update
- Close the Manage Environments dialog


## Future Work
Future work includes an Angular site where the downloaded images are displayed. 