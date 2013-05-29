/* Program client_distri.c.
	Get  and display distributed message 
	To be linked with DST_sock 
*/
#include <sys/types.h>   /* for type		*/
#include <sys/socket.h>  /* for socket API	*/
#include <netinet/in.h>  /* for address		*/
#include <arpa/inet.h>   /* for sockaddr_in */
#include <stdio.h>       /* for printf()	*/
#include <signal.h>      /* for CTRL+C Intrp*/
#include <stdlib.h>      /* for atoi()		*/
#include <string.h>      /* for strlen()	*/
#include <unistd.h>      /* for close()		*/

#define KB_size 80
#define EOB "&Q&&"
#define MaxString  50;
/***************************/
char *multiHostAddress = "239.255.10.10";
int T_over; //used for CTRL+C interrupt routine
int salesInterrupt = 0; //Use this to stop recieving multicast messages, if necessary
char userName[50]; //User inputted name
char massString[200]; //what to send out when making sale or bid

char *endFooter = "DST";

struct sockaddr_in server_EP; //Server Endpoint
int MC_socket; //Client Socket
int tcpPort = 7199; //TCP Port
char *serverMSG; //message from server

void forSale (int sock) 
{
	//first time it's connect
	serverMSG = (char *)readMessage (sock);
	//printf("Getting Server message = %s\n", serverMSG);

	if (serverMSG != NULL)
	{
		//printf ("Sending %s to server\n", massString);
	}
        sendtoDST (server_EP,sock, massString);
}   

int open_connect()
{
	int sock;
	int intPort;
	struct hostent *phe;
	int nc;
	 
	//printf("Creating TCP connection\n");
	sock = sockTCP_create();
	setHostByAddr (&server_EP, "any", tcpPort);
	sock_connect(sock, &server_EP); 
	//printf ("TCP Connection Established\n");
	//printf ("Wait for server message\n");
 
	return (sock);
}

void createTCPConnection()
{
	MC_socket = open_connect ();
	forSale (MC_socket);
}

//sets massString to have SALE parameters
void createAuction()
{
    char product[50];
    char bid[50];
    char closingDate[50];
    char closingTime[50];
    
    printf("\n****New Auction****\n");
    printf("Please enter auction product (no spaces): ");
    scanf("%s", product);
    printf("\nPlease enter initial bid: $");
    scanf("%s", bid);
    printf("\nPlease enter the closing date in the format dd/mm/yyyy\n");
    scanf("%s", closingDate);
    printf("\nPlease enter the closing time in 24-hour format hh:mm\n");
    scanf("%s", closingTime);
    //The time format is as follows: 1/11/2011-17:59
    strcpy(massString, "SALE");
    strcat(massString, "&");
    strcat(massString, userName); //seller
    strcat(massString, "&");
    strcat(massString, product);
    strcat(massString, "&");
    strcat(massString, bid);
    strcat(massString, "&");
    strcat(massString, closingDate);
    strcat(massString, "-");
    strcat(massString, closingTime);
    strcat(massString, "&");
    strcat(massString, "DST"); //need to check whether a null character exists here!!!
    //massString[strlen(massString)] = '\0'; //add null character at the end
    
}

//sets massString to have bid parameters
void makeBid()
{
    char seller[50];
    char product[50];
    char bid[50];
    char closingDate[50];
    char closingTime[50];
    
    printf("\n*****Make Bid******\n");
    printf("Please enter bidding product (no spaces): ");
    scanf("%s", product);
    printf("\nPlease enter product sellers name: ");
    scanf("%s", seller);
    printf("\nPlease enter your bid: $");
    scanf("%s", bid);
    
    strcpy(massString, "BID");
    strcat(massString, "&");
    strcat(massString, seller);
    strcat(massString, "&");
    strcat(massString, product);
    strcat(massString, "&");
    strcat(massString, bid);
    strcat(massString, "&");
    strcat(massString, userName);
    strcat(massString, "&");
    strcat(massString, "DST");
    //massString[strlen(massString)] = '\0'; //add null character at the end
}

//This is the CTRL+C event handler
void createSale() 
{
    salesInterrupt = 1; //Interrupt sales set to true
    int MC_socket;
    int functionSwitch;
    char *theSale;
    
    printf ("\n**************************************\n");
    printf ("      What would you like to do?\n");
    printf ("\nEnter 1 to create a new auction item");
    printf ("\nEnter 2 to make a bid");
    printf ("\n**************************************\n");

    //Scans the function option
    scanf("%d", &functionSwitch);
    
    switch(functionSwitch)
    {
        case 1:
        createAuction();
        break;

        case 2:
        makeBid();
        break;
    } 
    
    //printf("serverMessage: %s\n", massString);
    createTCPConnection();
}


void displaySale(char seller[], char product[], char bid[], char closingTime[])
{
    printf("\n******Auction Update******\n");
    printf("Product:         %s\n", product);
    printf("Current Bid:     $%s\n", bid);
    printf("Seller:          %s\n", seller);
    printf("Closing Time:    %s\n", closingTime);
    printf("\n**************************\n");
}

void displaySaleClosed(char seller[], char product[], char bid[], char closingTime[])
{
    printf("\n******Auction Closed******\n");
    printf("Product:         %s\n", product);
    printf("Final Bid:       $%s\n", bid);
    printf("Seller:          %s\n", seller);
    printf("Closing Time:    %s\n", closingTime);
    printf("\n**************************\n");
}

//Gets the sales made from server multicast and displays them
//also displays closed auctions from server multicast (FIN)
void getSalesMulticast(int sock)
{
	struct sockaddr_in read_EP;
	int stop = -1;
	char *msg;
	char *item;

	char header[10];
	char seller[20];
	char product[20];
	char bid[10];
	char buyer_closingTime[20];
	char footer[10];

	int i;
	int counter;

	int footerCheckValue;
	int headerCheckValue;

	printf("Recieving Multicasts from server...\n");
	printf("Please press CTRL+C for additional options\n");

	while (stop < 0) 
    {
        /*WARNING: Once inside CTRL+C interrupt, ALL unicast messages coming in will be HALTED
          until the CTRL+C program is complete, thus they will NOT display while your creating
          your auction*/
        (void) signal(SIGINT, createSale);
		msg = (char *)recvfromDST (&read_EP,sock); //THIS IS AN INTERRUPT
		
        if (msg != NULL)
		{
			//printf ("%s==>%s\n", inet_ntoa(read_EP.sin_addr),msg); //UDP message we recieved
			
            counter = 0;
			footerCheckValue = 0;
            
            //Store message into variables
			for(i = 0; i < strlen(msg); i++)
			{
				if(msg[i] == '&')
				{
					counter += 1;
				}
			}
            
			if(counter == 5)
			{
				item = strtok(msg, "&");
				strcpy(header, item);

				item = strtok(NULL, "&");
				strcpy(seller, item);

				item = strtok(NULL, "&");
				strcpy(product, item); 
                
                char temp[10]; //This is here to fix product changing to year bug
                strcpy(temp, product);
                
				item = strtok(NULL, "&");
				strcpy(bid, item);

				item = strtok(NULL, "&");
				strcpy(buyer_closingTime, item);

				item = strtok(NULL, "&");
				strcpy(footer, item);
                
                footerCheckValue = confirmFooter(footer);
				headerCheckValue = getOperation(header);
				
				//Display a sale item (new or being bidded upon)
				if(headerCheckValue == 2 && footerCheckValue == 3)
                {
                    displaySale(seller, temp, bid, buyer_closingTime);
                }
                
                if(headerCheckValue == 3 && footerCheckValue == 3)
                {
                    displaySaleClosed(seller, temp, bid, buyer_closingTime);
                }
			}
			
			
			
		}
		else stop = 0;
	}
}

//Returns 1 for BID, 2 for SALE and 3 for FIN
//returns 0 if none of the above
int getOperation(char operationString[])
{
    char bidHeader[] = "BID"; //NOTE we dont need this
	char saleHeader[] = "SALE";
	char finHeader[] = "FIN";

	char checker[6]; //checker contains TRUE value (eg not BID'/0')
	int i;
	int counter; //used to see how many values are correct

    //Is the operation BID?
	if(strlen(operationString) == strlen(bidHeader) && operationString[0] == bidHeader[0]) //checks for B as well (since BID and FIN are same length)
	{
        counter = 0;
        for(i = 0; i < strlen(bidHeader); i++)
        {
            if(operationString[i] == bidHeader[i])
			{
				counter += 1;
			}
        }
        
        if(counter == strlen(bidHeader))
        {
            return 1;
        }
	}
	
    //Is the operation SALE?
	if(strlen(operationString) == strlen(saleHeader)) //make sure lengths are the same
	{
        counter = 0;
        for(i = 0; i < strlen(saleHeader); i++)
        {
            if(operationString[i] == saleHeader[i])
			{
				counter += 1;
			}
        }
        
        if(counter == strlen(saleHeader))
        {
            return 2;
        }
	}
	
    //Is the operation FIN?
	if(strlen(operationString) == strlen(finHeader)) //make sure lengths are the same
	{
        counter = 0;
        for(i = 0; i < strlen(finHeader); i++)
        {
            if(operationString[i] == finHeader[i])
			{
				counter += 1;
			}
        }
        
        if(counter == strlen(finHeader))
        {
            return 3;
        }
	}
    
    printf("no match on BID, SALE or FIN with %s\n", operationString);
    return 0;	
}

//returns 3 if footer is DST
//else returns anything from 0 ~ 2
int confirmFooter(char stringCheck[])
{
	int i;
	int counter;
	char checker[] = "DST";
	counter = 0;
	
	if(strlen(stringCheck) == 3)
	{

		for(i = 0; i < 3; i++)
		{
			if(checker[i] == stringCheck[i])
			{
				counter += 1;
			}
		}
	}
	else
	{
		counter = 0;
	}
	return counter;
}


int main(int argc, char **argv)
{
	int sockMulti;
	int readPort;
	struct sockaddr_in EP_multi;
	struct ip_mreq request;
	char buf[40]= "Need port [IP], e.g.9001 [239.255.x.x]";
	char *msg;
	unsigned char TTL=3;
	

	argc = 4;
	argv[1] = "7198"; //read port
	argv[2] = "7199"; //write port
	argv[3] = "239.255.10.10";

	printf("\n\nAddress Type:%d \nRead Port: %s \nWrite Port: %s\nIP Address: %s\n", argc, argv[1], argv[2], argv[3]);

	printf("\n************************************************************\n");
	printf("Welcome to The Auction Software Package(POWERED BY CYCHROME)");
	printf("\n************************************************************\n\n");

	printf("Enter your name: ");

	//Reads the user's name
	scanf("%s", userName);

	printf("\n\nWelcome %s\n", userName);

	readPort = atoi (argv[1]);
	//if (argc == 3)
	//{
	//	multiHostAddress = argv[2];
	//}

	sockMulti = sockUDP_create();
	sockUDP_reuse (sockMulti);
	setHostByAddr (&EP_multi, "any", readPort);
	sock_bind (sockMulti, readPort);
	sockUDP_join (sockMulti, multiHostAddress,&request);

	getSalesMulticast(sockMulti);


	sockUDP_drop (sockMulti, request);
	close(sockMulti);
}
