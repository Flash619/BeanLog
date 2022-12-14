CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
    );

START TRANSACTION;

DO
$EF$
BEGIN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'identity') THEN
CREATE SCHEMA identity;
END IF;
END
$EF$;

REVOKE ALL ON SCHEMA identity FROM PUBLIC;

GRANT USAGE ON SCHEMA identity TO beanlog; -- TODO Maybe make this a different user for security.

CREATE TABLE identity.role (
                               id uuid NOT NULL,
                               name character varying(256) NULL,
                               normalized_name character varying(256) NULL,
                               concurrency_stamp text NULL,
                               CONSTRAINT pk_role PRIMARY KEY (id)
);

CREATE TABLE identity."user" (
                                 id uuid NOT NULL,
                                 user_name character varying(256) NULL,
                                 normalized_user_name character varying(256) NULL,
                                 email character varying(256) NULL,
                                 normalized_email character varying(256) NULL,
                                 email_confirmed boolean NOT NULL,
                                 password_hash text NULL,
                                 security_stamp text NULL,
                                 concurrency_stamp text NULL,
                                 phone_number text NULL,
                                 phone_number_confirmed boolean NOT NULL,
                                 two_factor_enabled boolean NOT NULL,
                                 lockout_end timestamp with time zone NULL,
                                 lockout_enabled boolean NOT NULL,
                                 access_failed_count integer NOT NULL,
                                 CONSTRAINT pk_user PRIMARY KEY (id)
);

CREATE TABLE identity.role_claim (
                                     id integer GENERATED BY DEFAULT AS IDENTITY,
                                     role_id uuid NOT NULL,
                                     claim_type text NULL,
                                     claim_value text NULL,
                                     CONSTRAINT pk_role_claim PRIMARY KEY (id),
                                     CONSTRAINT fk_role_claim_asp_net_roles_role_id FOREIGN KEY (role_id) REFERENCES identity.role (id) ON DELETE CASCADE
);

CREATE TABLE identity.user_claim (
                                     id integer GENERATED BY DEFAULT AS IDENTITY,
                                     user_id uuid NOT NULL,
                                     claim_type text NULL,
                                     claim_value text NULL,
                                     CONSTRAINT pk_user_claim PRIMARY KEY (id),
                                     CONSTRAINT fk_user_claim_asp_net_users_user_id FOREIGN KEY (user_id) REFERENCES identity."user" (id) ON DELETE CASCADE
);

CREATE TABLE identity.user_login (
                                     login_provider text NOT NULL,
                                     provider_key text NOT NULL,
                                     provider_display_name text NULL,
                                     user_id uuid NOT NULL,
                                     CONSTRAINT pk_user_login PRIMARY KEY (login_provider, provider_key),
                                     CONSTRAINT fk_user_login_user_user_id FOREIGN KEY (user_id) REFERENCES identity."user" (id) ON DELETE CASCADE
);

CREATE TABLE identity.user_role (
                                    user_id uuid NOT NULL,
                                    role_id uuid NOT NULL,
                                    CONSTRAINT pk_user_role PRIMARY KEY (user_id, role_id),
                                    CONSTRAINT fk_user_role_role_role_id FOREIGN KEY (role_id) REFERENCES identity.role (id) ON DELETE CASCADE,
                                    CONSTRAINT fk_user_role_user_user_id FOREIGN KEY (user_id) REFERENCES identity."user" (id) ON DELETE CASCADE
);

CREATE TABLE identity.user_token (
                                     user_id uuid NOT NULL,
                                     login_provider text NOT NULL,
                                     name text NOT NULL,
                                     value text NULL,
                                     CONSTRAINT pk_user_token PRIMARY KEY (user_id, login_provider, name),
                                     CONSTRAINT fk_user_token_user_user_id FOREIGN KEY (user_id) REFERENCES identity."user" (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX "RoleNameIndex" ON identity.role (normalized_name);

CREATE INDEX ix_role_claim_role_id ON identity.role_claim (role_id);

CREATE INDEX "EmailIndex" ON identity."user" (normalized_email);

CREATE UNIQUE INDEX "UserNameIndex" ON identity."user" (normalized_user_name);

CREATE INDEX ix_user_claim_user_id ON identity.user_claim (user_id);

CREATE INDEX ix_user_login_user_id ON identity.user_login (user_id);

CREATE INDEX ix_user_role_role_id ON identity.user_role (role_id);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20221127054356_112620222343-identity', '7.0.0');

REVOKE ALL ON ALL TABLES IN SCHEMA identity FROM PUBLIC;

GRANT
SELECT,
INSERT,
UPDATE,
DELETE
ON ALL TABLES IN SCHEMA identity TO beanlog;

COMMIT;