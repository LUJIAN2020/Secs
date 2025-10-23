## SECS 介绍

Secs库实现了HSMS，HSMS定义了使用 TCP/IP 作为物理传输媒质时的通信接口，使用TCP/IP流支持，提供了可靠的双向同步传输。
支持Active和Passive模式
- Active（Client端）时主动连接远端服务器
- Passive（Server端）时监听远端的客户端连接

### HSMS连接状态
- **NOT CONNECTED：**
该实体已准备好侦听或启动TCP/IP连接，但尚未建立任何连接，或所有以前建立的TCP/IP连接都已终止；
- **CONNECTED：**
已建立了一个TCP/IP连接。此状态有两个子状态，NOT SELECTED 和 SELECTED；
- **NOT SELECTED：**
未建立HSMS会话或任何先前建立的HSMS会话已结束；
- **SELECTED：**
至少建立了一个HSMS会话，这是HSMS的通常操作状态：数据消息可以在此状态下进行交换。

### 数据类型

|序号 |HSMS类型 |   C#数据类型  |            SML示例                 |
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

### 实例创建
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

### Options参数

|序号|参数               |   描述        | 
|----|------------------|---------------|
| 1  |SessionId         |DeviceId|    
| 2  |IP                |Active时为远端IP，Passive时为本地监听IP|   
| 3  |Port              |Active时为远端Port，Passive时为本地监听Port,默认：5000|     
| 4  |ConnectionMode    |Passive：处于被动模式的本地实体侦听并接受由处于主动模式的远程实体发起的连接。ActiveMode：由处于主动模式的本地实体发起连接。|        
| 5  |LinkTestInterval  |Linktest指令发送间隔，默认：60秒|         
| 6  |LinkTest          |true=发送Linktest,false=不发送Linktest，默认：false|
| 7  |T3                |回复超时，定义一个实体等待回复消息的最长时间，如果T3超时则取消这次会话事务但不断开 TCP/IP 连接。默认：45秒|
| 8  |T5                |连接间隔时间，定义两个连接请求之间的时间间隔。默认：10秒|
| 9  |T6                |控制事务超时，定义了一个控制事务所能保持开启的最长时间，超过该时间就认为这次通信失败。默认：5秒|
| 10 |T7                |未选择状态超时，定义当建立了 TCP/IP 连接之后通信处于 Not Selected 状态的最长时间，通信必须在该时间完成 Selected Procedure，否则将会断开 TCP/IP 连接。默认：10秒|
| 11 |T8                |网络字符超时，定义成功接收到单个HSMS 消息的字符之间的最大时间间隔。默认：5秒|

### 内部委托
- **SessionConnectionChanged** 连接节点变化委托
EndPoint? arg1 : 本地终结点
EndPoint? arg2 : 远程终结点
bool arg3 : true=连接，false=断开

- **InternalException** 内部异常委托
string？ arg1 : 消息
Exception arg2 : 异常

- **RawMessageChanged** 发送或接收原始数据发生变化时触发的委托
byte[] arg1 : 传输的数据
RawType arg2 ：发送/接收

- **HsmsDataContextChanged** Hsms上下文变化的委托
HsmsDataContext arg1 ：Hsms数据上下文

- **HsmsMessageChanged** Hsms消息变化委托
HsmsMessage arg1 : Hsms消息

- **ConnectionStateChanged** 连接状态变化的委托
ConnectionState arg1 : 连接状态

- **SubscribeRemoteCaller** 订阅远程调用委托，被动接收消息，业务处理主要委托
HsmsMessage arg1 : 远端发送的Hsms消息
HsmsMessage arg2 : 本地回复远端的Hsms消息

### 发送消息
```C#

//发送SML结构数据
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

//发送SML
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

//发送Linktest.req
var rsp = server.SendLinktestReq();

//发送Select.req
var rsp = server.SendSelectReq();

//发送Deselect.req
var rsp = server.SendDeselectReq();

//发送Reject.req
var rsp = server.SendRejectRsp();

//发送Separate.req
server.SendSeparateReq();

```