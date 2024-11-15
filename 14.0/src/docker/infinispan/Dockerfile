FROM quay.io/infinispan/server:14.0

ARG KEYCLOAK_VERSION=24.0.1

USER root

RUN microdnf update -y && microdnf install -y shadow-utils tar which && microdnf clean all

RUN groupadd -g 11000 "infinispanGroup"
RUN adduser \
    --uid 10000 \
    --gid 11000 \
    --password 'pwd' \
    --no-create-home \
    --shell /bin/bash \
    --comment "InfinispanAccount" \
    "infinispan_account"

#Copy security realms
COPY ./security/groups.properties /opt/infinispan/server/conf/groups.properties
COPY ./security/users.properties /opt/infinispan/server/conf/users.properties 

# Set permissions for the system folders
RUN chown -R 10000:11000 /bin
RUN chown -R 10000:11000 /boot
RUN chown -R 10000:11000 /dev
RUN chown -R 10000:11000 /home
RUN chown -R 10000:11000 /usr/lib
RUN chown -R 10000:11000 /usr/lib64
RUN chown -R 10000:11000 /usr/sbin
RUN chown -R 10000:11000 /media
RUN chown -R 10000:11000 /root
RUN chown -R 10000:11000 /run
RUN chown -R 10000:11000 /srv
RUN chown -R 10000:11000 /tmp
RUN chown -R 10000:11000 /var
RUN chown -R 10000:11000 /deployments
RUN chown -R 10000:11000 /home/jboss
RUN chown -R 10000:11000 /opt/infinispan

USER infinispan_account

WORKDIR /opt/infinispan

ENTRYPOINT ["./bin/launch.sh"]