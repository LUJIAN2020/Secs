## SECS Introduction

The Secs library implements HSMS, which defines the communication interface when using TCP/IP as the physical transmission medium.
It leverages TCP/IP stream support to provide reliable, bidirectional, and synchronous communication.
Both Active and Passive modes are supported:
- In Active mode (Client side), the system initiates connections to remote servers.

- In Passive mode (Server side), the system listens for incoming connections from remote clients.

### HSMS Connection States
**NOT CONNECTED:**
The entity is ready to listen for or initiate a TCP/IP connection, but no connection has been established, or all previously established TCP/IP connections have been terminated.

**CONNECTED:**
A TCP/IP connection has been established. This state includes two sub-states: NOT SELECTED and SELECTED.

**NOT SELECTED:**
An HSMS session has not been established, or any previously established HSMS session has ended.

**SELECTED:**
At least one HSMS session has been established. This is the normal operational state of HSMS, in which data messages can be exchanged.

### Data Type

| NO |HSMS Type |   C# Type   |            SML Sample                |
|----|---------|---------------|-----------------------------------|
| 1  |    L    |               | L[2] |
| 2  |    A    | string(ASCII) | A[6] "Abc123"|
| 3  |    B    | byte[]        | B[3] 0x01 0xAF 0x39|
| 4  | BOOLEAN | bool[]        | BOOLEAN[3] 0x00 0x01 0x00|
| 5  |    F4   | float         | F4[1] 1.23, F4[1] -1.23 |
| 6  |    F8   | double        | F8[1] 1.2345678, F8[1] -1.2345678|
| 7  |    I1   | sbyte         | I1[1] 120, I1[1] -120|
| 8  |    I2   | short         | I2[1] 21, I2[1] -21|
| 9  |    I4   | int           | I4[1] 900, I4[1] -900|
| 10 |    I8   | long          | I8[1] 90769, I8[1] -90769|
| 11 |    U1   | byte          | U1[1] 210|
| 12 |    U2   | ushort        | U2[1] 457|
| 13 |    U4   | uint          | U4[1] 6487|
| 14 |    U8   | ulong         | U8[1] 12345667|

### Creating Instances
```C#
var options = new HsmsOptions()
{
    SessionId = 0,
    IP = "127.0.0.1",
    Port = 5000,
    ConnectionMode = ConnectionMode.Active,         
    LinkTest = false, 
};
var server = new HsmsServer(options)
{
    InternalException = OnInternalException,
    RawMessageChanged = OnRawMessageChanged,
    HsmsDataContextChanged = OnHsmsDataContextChanged,
    HsmsMessageChanged = OnHsmsMessageChanged,
    SessionConnectionChanged = OnSessionConnectionChanged,
    SubscribeRemoteCaller = OnSubscribeRemoteCaller,
    ConnectionStateChanged = OnConnectionStateChanged
};
server.Start();
```

### Options Parameter

|No|Parameter               |   Description        | 
|----|------------------|---------------|
| 1  |SessionId         |DeviceId|    
| 2  |IP                |In Active mode, the remote IP is used; in Passive mode, the local listening IP is used.|   
| 3  |Port              |In Active mode, the remote port is used; in Passive mode, the local listening port is used. Default: 5000.|     
| 4  |ConnectionMode    |Passive:The local entity in passive mode listens for and accepts connections initiated by a remote entity operating in active mode. Active Mode:The local entity in active mode initiates a connection to the remote entity.|        
| 5  |LinkTestInterval  |Linktest command send interval, default: 60 seconds.|         
| 6  |LinkTest          |If set to true, Linktest commands will be sent periodically. If false, no Linktest will be sent. Default value: false.|
| 7  |T3                |Reply Timeout:Defines the maximum time an entity waits for a reply message.If T3 times out, the current session transaction is canceled, but the TCP/IP connection remains open.Default: 45 seconds.|
| 8  |T5                |Connection Interval:Defines the time interval between two connection attempts.Default: 10 seconds.|
| 9  |T6                |Control Transaction Timeout:Defines the maximum duration a control transaction can remain open.If this time is exceeded, the transaction is considered failed.Default: 5 seconds.|
| 10 |T7                |Not Selected Timeout:Defines the maximum duration the communication can remain in the Not Selected state after a TCP/IP connection is established.The communication must complete the Selected Procedure within this time, otherwise the TCP/IP connection will be disconnected.Default: 10 seconds.|
| 11 |T8                |Network Character Timeout:Defines the maximum time interval between characters when successfully receiving a single HSMS message.Default: 5 seconds.|

### Subscribe Message
- **SessionConnectionChanged** Connection Node Changed Delegate
EndPoint? arg1 : Local EndPoint
EndPoint? arg2 : Remote EndPoint
bool arg3 : true = connect, false = disconnect.

- **InternalException** Internal Exception Delegate
string？ arg1 : Message
Exception arg2 : Exception

- **RawMessageChanged** Delegate triggered when raw data is sent or received
byte[] arg1 : Transmitted Data
RawType arg2 ：sent or received

- **HsmsDataContextChanged** HSMS Context Changed Delegate
HsmsDataContext arg1 ：Hsms data context

- **HsmsMessageChanged** HSMS Message Changed Delegate
HsmsMessage arg1 : Hsms Message

- **ConnectionStateChanged** Connection Status Changed Delegate
ConnectionState arg1 : ConnectionState

- **SubscribeRemoteCaller** Subscribe Remote Invocation Delegate: A delegate used to passively receive messages from a remote entity. It serves as the primary delegate for handling business logic.
HsmsMessage arg1 : HSMS Message Sent from Remote
HsmsMessage arg2 : HSMS Message Response to Remote

### Send Message
```C#

//Send SML Structured Data
var body = new L
    (
        new A("Abc123"),
        new U1(1),
        new U2(2),
        new U4(4),
        new U8(8),
        new I1(-1),
        new I1(1),
        new I2(-2),
        new I2(2),
        new I4(-4),
        new I4(4),
        new I8(-8),
        new I8(8),
        new F4(-1.23456f),
        new F4(1.23456f),
        new F8(-1.23456),
        new F8(1.23456),
        new L
        (
            new L
            (
                new A("Abc123"),
                new U1(1),
                new U2(2),
                new U4(4),
                new U8(8),
                new I1(-1),
                new I1(1),
                new I2(-2),
                new I2(2),
                new I4(-4),
                new I4(4),
                new I8(-8),
                new I8(8),
                new F4(-1.23456f),
                new F4(1.23456f),
                new F8(-1.23456),
                new F8(1.23456)
            ),
            new L
            (
                new A("Abc123"),
                new U1(1),
                new U2(2),
                new U4(4),
                new U8(8),
                new I1(-1),
                new I1(1),
                new I2(-2),
                new I2(2),
                new I4(-4),
                new I4(4),
                new I8(-8),
                new I8(8),
                new F4(-1.23456f),
                new F4(1.23456f),
                new F8(-1.23456),
                new F8(1.23456)
            )
        )
    );
var response = server.SendDataMessage(2, 41, body);

//Send SML
string sml = @"
<L[18]
    <A[6] ""Abc123"">
    <U1[1] 1>
    <U2[1] 2>
    <U4[1] 4>
    <U8[1] 8>
    <I1[1] -1>
    <I1[1] 1>
    <I2[1] -2>
    <I2[1] 2>
    <I4[1] -4>
    <I4[1] 4>
    <I8[1] -8>
    <I8[1] 8>
    <F4[1] -1.23456>
    <F4[1] 1.23456>
    <F8[1] -1.23456>
    <F8[1] 1.23456>
    <L[2]
        <L[17]
            <A[6] ""Abc123"">
            <U1[1] 1>
            <U2[1] 2>
            <U4[1] 4>
            <U8[1] 8>
            <I1[1] -1>
            <I1[1] 1>
            <I2[1] -2>
            <I2[1] 2>
            <I4[1] -4>
            <I4[1] 4>
            <I8[1] -8>
            <I8[1] 8>
            <F4[1] -1.23456>
            <F4[1] 1.23456>
            <F8[1] -1.23456>
            <F8[1] 1.23456>
        >
        <L[19]
            <A[6] ""Abc123"">
            <U1[1] 1>
            <U2[1] 2>
            <U4[1] 4>
            <U8[1] 8>
            <I1[1] -1>
            <I1[1] 1>
            <I2[1] -2>
            <I2[1] 2>
            <I4[1] -4>
            <I4[1] 4>
            <I8[1] -8>
            <I8[1] 8>
            <F4[1] -1.23456>
            <F4[1] 1.23456>
            <F8[1] -1.23456>
            <F8[1] 1.23456>
            <B[3] 0x13 0xAB 0x1F>
            <BOOLEAN[2] 0x00 0x01>
        >
    >
>";
var response = server.SendDataMessage(2, 41, sml);

//Send Linktest.req
var rsp = server.SendLinktestReq();

//Send Select.req
var rsp = server.SendSelectReq();

//Send Deselect.req
var rsp = server.SendDeselectReq();

//Send Reject.req
var rsp = server.SendRejectRsp();

//Send Separate.req
server.SendSeparateReq();

```