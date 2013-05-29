/*Program server_distri.c.
	To be linked with DST_sock 
	Read messages from a TCP user and
	multicast them to multi-clients
*/

#include <sys/types.h>   /* for type		*/
#include <sys/socket.h>  /* for socket API  */
#include <netinet/in.h>  /* for address		*/
#include <arpa/inet.h>   /* for sockaddr_in */
#include <stdio.h>       /* for printf()	*/
#include <stdlib.h>      /* for atoi()		*/
#include <string.h>      /* for strlen()	*/
#include <unistd.h>      /* for close()		*/
#include <time.h>

#define maxStringSize  200
#define maxSaleItems  20
/***************************/

//Define Product bidding structure
typedef struct
{
    char seller[maxStringSize];
    char product[maxStringSize];
    char initBid[maxStringSize];
    time_t closingTime;    
	int isClosed;
}Saleitem;

Saleitem saleItems[maxSaleItems];
int SaleId = 0; //used to kep track of multiple records
int sendFinMessage = 0;//

char massString[maxStringSize];

int readPort, multiPort;
char *multiHostAddr = "239.255.10.10";

//Creates a time structure with set time parameters
//and returns it
time_t createTime(int day, int month, int year, int hour, int minutes, int seconds)
{
    struct tm t;
    time_t time;

	t.tm_year = year-1900;// current year-1900
	t.tm_mon = month-1; // month of year [0-11]
	t.tm_mday = day;// day of the month [1-31]
	
	t.tm_hour = hour; // hours after midnight [0-23]
	t.tm_min = minutes; // minutes after hour [0-59]
	t.tm_sec = seconds;//*seconds after minute [0-61] (61 allows for 2 leap-seconds)
	t.tm_isdst = 1;//* daylight savings indicator
	time = mktime(&t);
	return time;
}


//Stores all sale item variables from a massString
//seperated by &
void storeSaleItem(char massString[])
{
    char *item;
    
    int minutes;
    int hours;
    int days;
    int months;
    int years;
    //SALE&Cameron&frog&20.00&23/11/2011-23:00&DST
    item = strtok(massString,"&"); //SALE
    
    item = strtok(NULL,"&"); //seller 
    strcpy(saleItems[SaleId].seller, item); //printf("%s\n", item);
        
    item = strtok(NULL,"&"); //product
    strcpy(saleItems[SaleId].product, item);// printf("%s\n", item);
        
    item = strtok(NULL,"&"); //price
    strcpy(saleItems[SaleId].initBid, item); // printf("%s\n", item);
    
    //Get the date variables
    item = strtok(NULL,"/"); //day
    days = atoi(item);  //printf("%s\n", item);
    item = strtok(NULL,"/"); //month
    months = atoi(item);  //printf("%s\n", item);
    item = strtok(NULL,"-"); //year
    years = atoi(item);  //printf("%s\n", item);
    
    //get the time variables
    item = strtok(NULL,":"); //year
    hours = atoi(item);  //printf("%s\n", item);
    
    item = strtok(NULL,"&"); //year
    minutes = atoi(item);  //printf("%s\n", item);
    
     //Create time here
     saleItems[SaleId].closingTime = createTime(days, months, years, hours, minutes, 0); //WHY THEY ALL THE SAME?!?!?!?!
    SaleId++;//
}

//Updates a saleItem with a new bid
void updateBid(char newBid[], int index)
{
    strcpy(saleItems[index].initBid, newBid);
}

//If the new nid is less than current bid return 0
//else returns 1
int isBidValid(char newBid[], int saleIndex)
{
    float valOne;
    float valTwo;
    
    valOne = atof(newBid);
    valTwo = atof(saleItems[saleIndex].initBid);
    
    if(valOne < valTwo)
    {
        return 0;
    }
    
    return 1;
}

//Gets the index of the sale item
//which has matching Seller and Product names in massString
//If not found return -1
//if the bid is too small or sale is closed, return -1
int getSaleItemIndex(char massString[])
{
    //TODO
    char *item;
    char theSeller[maxStringSize];
    char theProduct[maxStringSize];
    char theBid[maxStringSize];
    
    //SALE&Cameron&frog&20.00&23/11/2011-23:00&DST
    item = strtok(massString,"&"); //BID
    
    item = strtok(NULL,"&"); //seller 
    strcpy(theSeller, item);
        
    item = strtok(NULL,"&"); //product
    strcpy(theProduct, item);
        
    item = strtok(NULL,"&"); //offer bid
    strcpy(theBid, item);
    
    int i;
    int thisSeller;
    int thisProduct;
    int validate;
    for(i = 0; i < SaleId; i++)
    {
        thisSeller = (strcmp (theSeller, saleItems[i].seller) == 0);
        thisProduct = (strcmp (theProduct, saleItems[i].product) == 0);
        if((thisSeller == 1) && (thisProduct == 1) && (saleItems[i].isClosed) != 1)
        {
            printf("match found at index %d\n", i);
            
            validate = isBidValid(theBid, i);
            
            if(validate == 1)
            {
                updateBid(theBid, i);
            }
            else
            {
                printf("Bid %s is invalid, needs to be higher than current bid\n", theBid);
                return -1;
            }

            return i;
        }
    }
    printf("Product '%s' with seller '%s' could not be found\n", theProduct, theSeller);
    return -1;
}

//creates the SALE string and
//stores it in the global massString variable
void createSaleString(int index)
{
    strcpy(massString, "SALE");
    strcat(massString, "&");
    strcat(massString, saleItems[index].seller);
    strcat(massString, "&");
    strcat(massString, saleItems[index].product);
    strcat(massString, "&");
    strcat(massString, saleItems[index].initBid);
    
    strcat(massString, "&");
    //create time string
    //time(&saleItems[index].closingTime);

    char timeString[maxStringSize]; 
    int timeLength = strlen(ctime(&saleItems[index].closingTime));
    strncpy(timeString, ctime(&saleItems[index].closingTime), timeLength-1);
    timeString[timeLength-1] = '\0';

    strcat(timeString, "&");
    strcat(timeString, "DST");
    strcat(massString, timeString);
}

//creates the SALE string and
//stores it in the global massString variable
void createFinString(int index)
{
    strcpy(massString, "FIN");
    strcat(massString, "&");
    strcat(massString, saleItems[index].seller);
    strcat(massString, "&");
    strcat(massString, saleItems[index].product);
    strcat(massString, "&");
    strcat(massString, saleItems[index].initBid);
    
    strcat(massString, "&");
    //create time string
    //time(&saleItems[index].closingTime);

    char timeString[maxStringSize]; 
    int timeLength = strlen(ctime(&saleItems[index].closingTime));
    strncpy(timeString, ctime(&saleItems[index].closingTime), timeLength-1);
    timeString[timeLength-1] = '\0';

    strcat(timeString, "&");
    strcat(timeString, "DST");
    strcat(massString, timeString);
}

//displays TIMES UP when time is equal or less than
void timesUp(time_t start, time_t end, int index)
{
    float difference = difftime(end, start);
	//printf("current time: %s\n", ctime(&start));
	//printf("close time: %s\n", ctime(&end));
    if(difference <= 0) //if time has passed or equal closing time
	{
        if(saleItems[index].isClosed != 1) //if sale still open
        {
            createFinString(index);
            saleItems[index].isClosed = 1; //set sale to closed
            sendFinMessage = 1; //set send pointer on
        }
    }
}

void checkFinishedBids(time_t currentTime)
{
	int i;

	for(i = 0; i < SaleId; i++)
	{
		timesUp(currentTime, saleItems[i].closingTime, i);
	}
}

void multicastService (EP_multi, multiSock) 
	struct sockaddr_in EP_multi; 
	int multiSock;	

{
	const char *EOBPtr = "QUIT";
	const char *stopMsgPtr = "SerEnd&&";
	const char *salerStart = "start sale";
	const char *salerStop = "stop sale";
	const char *saleHeader = "SALE";
	const char *bidHeader = "BID";
	const char *finHeader = "FIN";
	char userBuf[1024] ="Enter message to unicast, 'QUIT' to disconnect, or 'SerEnd&&' to stop multicast";
	char *msgPtr ;
	int MS_socket, CS_socket;
	struct sockaddr_in EP_user;
	int stopServer = 0;
	int userQuit = 0;
	int isSale = 0; //checks for the SALE header
	int isBid = 0;
	char *tokPtr;

	MS_socket = sockTCP_create ();
	sock_bind (MS_socket, readPort);
	sock_listen (MS_socket, 0);
	sendtoDST (EP_multi, multiSock,  "Multicast started");

	while (stopServer == 0) //server running
	{
		
        //IN here: get current time
        //for each sale item, if current time == close time - send FIN message for that one bro

		CS_socket = sock_accept(MS_socket, &EP_user);

		if (CS_socket > 0)
		{
            printf("\nnew socket has been accepted \n");
			msgPtr = userBuf;
			sendtoDST (EP_user, CS_socket, msgPtr); //send the annoying welcome message
			msgPtr = (char *)getIPByEP(EP_user);
			sendtoDST (EP_multi, multiSock, msgPtr);
			sendtoDST (EP_multi, multiSock, salerStart); //sale start message sent
			userQuit = 0;

			while ((userQuit == 0) &&(stopServer == 0))	//socket running
			{   
                time_t now;
				time(&now);
			
				checkFinishedBids(now);
				
				if(sendFinMessage == 1)
				{
					sendFinMessage = 0; //set back to false
					printf("\nSending auction close message: %s\n", massString);
					sendtoDST (EP_multi, multiSock, massString);
				}

				msgPtr = (char *)readMessage(CS_socket); //read in the line, 
				printf("recieved: %s\n", msgPtr);
                sock_close(CS_socket); printf("Socket has now been closed\n");
                
				if (msgPtr != NULL)
				{
					if (strlen(msgPtr) == 0)
					{
						sock_close (CS_socket);
						userQuit = 1;
					}
					else
					{
						stopServer = (strncmp (msgPtr, stopMsgPtr, 8) == 0);
						
                        if(stopServer == 0)
						{
                            //CODE GOES HERE BRAH

							//sale function
							isSale = (strncmp (msgPtr, saleHeader, 4) == 0);
							if(isSale != 0) //this means true
							{
                                printf("entered SALE function\n");
                                storeSaleItem(msgPtr);
                                createSaleString(SaleId-1); //displays newly created sale string
                                printf("Sending '%s' through multicast\n", massString);
                                sendtoDST (EP_multi, multiSock, massString);
                                printf("Succesfully sent multicast\n");
                            }
                            
                            //bid function
                            isBid = (strncmp (msgPtr, bidHeader, 3) == 0);
							if(isBid != 0) //this means true
							{
                                printf("entered BID function\n");
                                int bidIndex = getSaleItemIndex(msgPtr);

                                if(bidIndex != -1)
                                {
                                    createSaleString(bidIndex);
                                    printf("Sending '%s' through multicast\n", massString);
                                    sendtoDST (EP_multi, multiSock, massString);
                                    printf("Succesfully sent multicast\n");
                                }
                            }
                            
                            
                            userQuit = 1;
						}
						else
						{
							sendtoDST (EP_user, CS_socket, "Thanks!");
							sendtoDST(EP_multi, multiSock, "Multicasting stopped");
							sock_close (CS_socket);
							sock_close (MS_socket);
						}
					}
				} 
                else
                {
                    userQuit = 1;
                }
				if (userQuit ==1)
				{
					sendtoDST (EP_multi, multiSock,salerStop); //sale stop message sent
				}
				
				//end of while loop
			}
		}
	}
}
/***************************/
int main(int argc, char **argv) {

  int sockMulti;             
  struct sockaddr_in EP_multi; 
  unsigned char TTL=3;   
  
  	argc = 4;
	argv[1] = "7199"; //read port
	argv[2] = "7198"; //write port
	argv[3] = "239.255.10.10";
printf("\n\nAddress Port:%d \n Read Port: %s \n Write Port: %s\n IP Address: %s\n", argc, argv[1], argv[2], argv[3]);
	
    if (argc < 3){
	  IO_error(1,"Need read and write ports e.g server 9000 9001 [239.255.x.x]");
	}  else {
	  readPort = atoi (argv[1]);
	  multiPort = atoi (argv[2]);	
	  if (argc > 3) {
		  multiHostAddr = argv[3];
	  }	
	  printf ("Multicast to IP address ");
	  printf ("%s at port",multiHostAddr);
	  printf (" %d \n", multiPort);
	  printf ("Users send multicast ");
	  printf ("messages to TCP port ");
	  printf ("%d \n", readPort);
 	  setHostByAddr (&EP_multi, 
		         multiHostAddr, multiPort);
	  sockMulti = sockUDP_create();
	  sockUDP_scope (sockMulti, TTL);
   	  multicastService (EP_multi,sockMulti);
	  close(sockMulti);
	}
}

