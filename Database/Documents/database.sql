drop table testdb.ReqLog;
create table testdb.ReqLog
(
    Id        char(36) charset ascii not null
        primary key,
    Url       varchar(256)           not null,
    UserId    char(36) charset ascii null,
    InTime    bigint                 null,
    OutTime   bigint                 null,
    Consuming bigint                 null
);

