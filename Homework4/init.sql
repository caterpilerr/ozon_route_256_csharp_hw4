CREATE TABLE "order" (
    id uuid,
    client_id bigint not null ,
    created_at timestamp with time zone not null ,
    delivered_at timestamp with time zone not null ,
    status int not null ,
    storehouse_id bigint,
    goods json not null,
    PRIMARY KEY (id, storehouse_id)
) PARTITION BY LIST (storehouse_id);

CREATE TABLE warehouse_0 PARTITION OF "order" FOR VALUES IN (0);
CREATE TABLE warehouse_1 PARTITION OF "order" FOR VALUES IN (1);
CREATE TABLE warehouse_2 PARTITION OF "order" FOR VALUES IN (2);



