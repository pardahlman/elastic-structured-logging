# Presentation

```
> cd ./presentation
> npm install
> npm start
```

_Psst!_ pressing `s` will open [Speakers notes](https://github.com/hakimel/reveal.js/#speaker-notes).

# Setup Elastic

Make sure you have [Docker isntalled](https://www.docker.com/get-docker), then run

```
> cd ./elastic
> docker-compose up -d
```

Once the containers are up and running, Elasticsearch will be available at `localhost:9200` and Kibana at `localhost:5601`. No authentication is needed.

For a complete Elastic Stack setup in docker, see [`elastic/stack-docker`](https://github.com/elastic/stack-docker).

## Visualize tag statistics

Create search `question-tag` with the following syntax

```
{
  "exists": {
    "field": "fields.questionTag"
  }
}
```

Create a visualization with a bucket split on `Terms` using the field `fields.questionTag.keyword` ([more info on keywords](https://www.elastic.co/guide/en/elasticsearch/reference/master/keyword.html)).

# Generate log events

Make sure Elasticsearch is running at `localhost:9200` without any authentication.

```
> cd ./dotnet-examples/Scenarios
> dotnet run
```

Head over to Kibana.

# Further reading

* https://messagetemplates.org/
* https://gregoryszorc.com/blog/2012/12/06/thoughts-on-logging---part-1---structured-logging/
* https://www.elastic.co/products
* https://serilog.net/
